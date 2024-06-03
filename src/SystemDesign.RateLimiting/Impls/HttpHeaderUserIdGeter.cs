namespace SystemDesign.RateLimiting.Impls
{
    /// <summary>
    ///     Получение id пользователя из заголовка запроса.
    /// </summary>
    /// <remarks>
    ///     Создание экземпляра класса <see cref="HttpHeaderUserIdGeter"/>.
    /// </remarks>
    /// <param name="accessor">Доступ к http context</param>
    public class HttpHeaderUserIdGeter(IHttpContextAccessor accessor) : IUserIdGeter
    {
        private readonly IHttpContextAccessor _contextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

        /// <inheritdoc/>
        public string Id
        {
            get
            {
                if (!_contextAccessor.HttpContext.Request.Headers.TryGetValue("user-id", out var value))
                    return string.Empty;
                return value.FirstOrDefault() ?? string.Empty;
            }
        }
    }
}
