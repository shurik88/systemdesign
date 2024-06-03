namespace SystemDesign.RateLimiting.RateLimit
{
    /// <summary>
    ///     Ограничитель трафика.
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        ///     Попытаться выполнить действие.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="action">Действие</param>
        /// <returns>Результат ограничителя</returns>
        Task<RateLimiterResult> TryDoActionAsync(string user, RateLimiterAction action);
    }
}
