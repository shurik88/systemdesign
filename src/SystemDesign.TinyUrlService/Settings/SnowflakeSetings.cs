namespace SystemDesign.TinyUrlService.Settings
{
    /// <summary>
    ///     Параметры генерации для снежинки.
    /// </summary>
    public class SnowflakeSetings
    {
        /// <summary>
        ///     Ид воркера(процесса).
        /// </summary>
        public int Worker { get; set; }

        /// <summary>
        ///     Ид дата центра.
        /// </summary>
        public int Datacenter { get; set; }
    }
}
