namespace SystemDesign.RateLimiting
{
    /// <summary>
    ///     Получатель идентификатора пользователя.
    /// </summary>
    public interface IUserIdGeter
    {
        /// <summary>
        ///     Ид пользовтаеля.
        /// </summary>
        string Id { get; }
    }
}
