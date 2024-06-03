using StackExchange.Redis;

namespace SystemDesign.RateLimiting.Redis
{
    public class RedisConnection
    {
        private readonly ConnectionMultiplexer _multiplexer;
        private RedisConnection(string connectionString)
        {
            _multiplexer = ConnectionMultiplexer.Connect(connectionString);
            
        }

        public IDatabase Database => _multiplexer.GetDatabase();

        public static RedisConnection Init(string connectionString)
        {
            return new RedisConnection(connectionString);
        }
    }
}
