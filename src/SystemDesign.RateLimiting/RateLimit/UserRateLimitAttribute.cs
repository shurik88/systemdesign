namespace SystemDesign.RateLimiting.RateLimit
{
    /// <summary>
    ///     Атрибут ограничения трафика.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UserRateLimitAttribute : Attribute
    {
        /// <summary>
        ///     Период на который распространяется правило.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        ///     Действие(правило).
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     Сколько всего возможно запросов выпонлить за указанный период.
        /// </summary>
        public int Total { get; set; }
    }
}
