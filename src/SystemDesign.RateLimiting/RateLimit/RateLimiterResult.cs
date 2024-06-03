namespace SystemDesign.RateLimiting.RateLimit
{
    /// <summary>
    ///     Результат работы ограничителя трафика.
    /// </summary>
    public class RateLimiterResult
    {
        /// <summary>
        ///     Оставшееся кол-во вызовов в интервале.
        /// </summary>
        public int Remain { get; set; }

        /// <summary>
        ///     Всего доступно вызовов в интервале.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        ///     Кол-во миллисекунд, после которого можно будет выполнить вызов.
        /// </summary>
        public long After { get; set; }

        /// <summary>
        ///     Можно ли выполнить операцию.
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
