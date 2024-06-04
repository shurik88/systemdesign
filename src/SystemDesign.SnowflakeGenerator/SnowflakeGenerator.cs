namespace SystemDesign.SnowflakeGenerator
{
    /// <summary>
    ///     Генерация числового идентификатора по алгоритму Snowflake.
    /// </summary>
    /// <remarks>
    ///     Создание экземпляра класса <see cref="SnowflakeGenerator"/>.
    /// </remarks>
    /// <param name="datacenterId">Ид датацентра</param>
    /// <param name="workerId">Ид рабочего процесса(сервиса)</param>
    public class SnowflakeGenerator(int datacenterId, int workerId)
    {
        private long _lastTime = 0;
        private readonly long _datacenterId = datacenterId;
        private readonly long _workerId = workerId;
        private long _counter = 1;
        private static readonly DateTime StartDate = new(2000, 1, 1, 0, 0, 1, DateTimeKind.Utc);
        private readonly object lockObj = new();

        
        /// <summary>
        ///     Получить новый идентификатор.
        /// </summary>
        /// <remarks>
        ///     1 бит отдается под знак(не используется)
        ///     41 - под кол-во мс
        ///     10 - под сервер (5 под датацентр, 5 под рабочий процесс-микрсоервис)
        ///     12 - под последовательность
        /// </remarks>
        /// <returns>Идентификатор</returns>
        public long GetId()
        {
            var diff = GetTimeComponent();
            lock(lockObj)
            {

                if (_lastTime == diff)
                {
                    //если переполнение.
                    if((_counter + 1) >= 4095)
                    {
                        while(diff == _lastTime)
                            diff = GetTimeComponent();
                        _counter = 1;
                    }
                    else
                        _counter++;
                }
                else
                    _counter = 1;

                _lastTime = diff;

                long idTime = diff << (64 - 1 - 41);
                long idTimeWithDatacenterId = idTime | _datacenterId << (64 - 1 - 41 - 5);
                long withoutCounter = idTimeWithDatacenterId | _workerId << (64 - 1 - 41 - 5 - 5);
                long id = withoutCounter | _counter << (64 - 1 - 41 - 5 - 5 - 12);
                return id;
            }
        }

        private long GetTimeComponent()
        {
            var now = DateTime.UtcNow;
            var diff = Convert.ToInt64((now - StartDate).TotalMilliseconds);
            return diff;
        }
    }
}
