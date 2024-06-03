namespace SystemDesign.RateLimiting.RateLimit
{
    /// <summary>
    ///     Действие.
    /// </summary>
    public class RateLimiterAction
    {
        /// <summary>
        ///     Название операции.
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        ///     Период на который распространяется правило.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        ///     Сколько всего возможно запросов выпонлить за указанный период.
        /// </summary>
        public int Total { get; set; }
    }
}
