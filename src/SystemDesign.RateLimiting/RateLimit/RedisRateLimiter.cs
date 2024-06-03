
using StackExchange.Redis;
using SystemDesign.RateLimiting.Redis;

namespace SystemDesign.RateLimiting.RateLimit
{
    /// <summary>
    ///     Ограничитель api-запросов на основе redis sorted sets.
    /// </summary>
    /// <remarks>
    ///     Создание экземпляра класса <see cref="RedisRateLimiter"/>.
    /// </remarks>
    /// <param name="connection">Соединение с redis</param>
    public class RedisRateLimiter(RedisConnection connection) : IRateLimiter
    {
        private readonly RedisConnection _connection = connection;
        private static readonly DateTime StartDate = new DateTime(2024, 1, 1, 0, 0, 1, DateTimeKind.Utc);

        private string GetKeyName(string user, string actionName) => $"ratelimit-{actionName}-{user}";

        /// <inheritdoc/>
        public async Task<RateLimiterResult> TryDoActionAsync(string user, RateLimiterAction action)
        {
            //var userId = geter.Id;
            var key = GetKeyName(user, action.Operation);

            var now = DateTime.UtcNow;
            var ms = (long)(now - StartDate).TotalMilliseconds;
            var oneIntervalAgo = ms - action.Interval * 1000;

            var comitted = false;
            //long count = 0;
            RedisValue[] redisValues = null;
            var sortedSetEntry = new SortedSetEntry(ms, ms);
            var db = _connection.Database;
            do
            {
                var tran = _connection.Database.CreateTransaction();
                var removeRes = tran.SortedSetRemoveRangeByScoreAsync(key, 0, oneIntervalAgo);
                var addSetTask = tran.SortedSetAddAsync(key, [sortedSetEntry]);
                var setRes = await _connection.Database.KeyExpireAsync(key, now.AddSeconds(action.Interval * 2));
                ////count = await tran.SortedSetLengthAsync(collection);
                var redisValuesTask = tran.SortedSetRangeByRankAsync(key, 0, -1);
                comitted = await tran.ExecuteAsync();
                if (comitted)
                    redisValues = redisValuesTask.Result;

                //without transaction
                //await db.SortedSetRemoveRangeByScoreAsync(key, 0, oneIntervalAgo);
                //await db.SortedSetAddAsync(key, [sortedSetEntry]);
                //await _connection.Database.KeyExpireAsync(key, now.AddSeconds(action.Interval * 2));
                //redisValues = await db.SortedSetRangeByRankAsync(key, 0, -1);
                //comitted = true;
            }
            while (!comitted);

            if (redisValues.Length > action.Total)
            {
                var sorted = redisValues.OrderBy(x => x).ToList();
                var oldLast = Convert.ToInt64(sorted.Skip(redisValues.Length + 1 - action.Total).First());
                var after = action.Interval * 1000 - (ms - oldLast);
                return new RateLimiterResult
                {
                    Remain = 0,
                    IsSuccess = false,
                    Total = action.Total,
                    After = after
                };
            }
            else
                return new RateLimiterResult
                {
                    IsSuccess = true,
                    Total = action.Total,
                    Remain = action.Total - redisValues.Length
                };
        }
    }
}
