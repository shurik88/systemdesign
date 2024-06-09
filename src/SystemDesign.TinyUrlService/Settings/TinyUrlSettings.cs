namespace SystemDesign.TinyUrlService.Settings
{
    /// <summary>
    ///     Параметры сокращения адресов.
    /// </summary>
    public class TinyUrlSettings
    {
        /// <summary>
        ///     Доступный алфавит.
        /// </summary>
        public string Alphabet { get; set; }

        /// <summary>
        ///     Параметры генерации для алдгоритма "Снежинка".
        /// </summary>
        public SnowflakeSetings Snowflake { get; set; }
    }
}
