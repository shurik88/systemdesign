namespace SystemDesign.TinyUrlService.Services
{
    /// <summary>
    ///     Генератор коротких ссылок.
    /// </summary>
    public class TinyUrlGenerator
    {
        private readonly string _alphabet;

        /// <summary>
        ///     Создание экземпляра класса <see cref="TinyUrlGenerator"/>.
        /// </summary>
        /// <param name="alphabet">Алфавит</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TinyUrlGenerator(string alphabet)
        {
            if (string.IsNullOrEmpty(alphabet))
                throw new ArgumentNullException(nameof(alphabet));

            _alphabet = alphabet;
        }

        /// <summary>
        ///     Генерация короткой ссылки.
        /// </summary>
        /// <param name="number">Число на основе которого генерируется ссылка</param>
        /// <returns>Часть ссылки</returns>
        public string Generate(long number)
        {
            var symbols = new List<char>();
            var remain = number;
            while(remain != 0)
            {
                long mod = remain % _alphabet.Length;
                symbols.Add(_alphabet[Convert.ToInt32(mod)]);
                remain /= _alphabet.Length;
            }
            symbols.Reverse();
            return new string(symbols.ToArray());
        }

        /// <summary>
        ///     Получение числа на основе ссылки
        /// </summary>
        /// <param name="tiny">Сокращенная ссылка</param>
        /// <returns>Число | null если строка некорректная</returns>
        public long? GetBaseNumberFromString(string tiny)
        {
            if (string.IsNullOrEmpty(tiny))
                return null;

            if (tiny.Any(x => _alphabet.IndexOf(x) == -1))
                return null;
            long number = 0;

            try
            {
                checked
                {
                    for(var j = tiny.Length - 1; j >= 0; j--)
                    {
                        var index = _alphabet.IndexOf(tiny[j]);
                        number += GetPow(tiny.Length - 1 - j) * index;
                    }
                }
            }
            catch(System.OverflowException)
            {
                return null;
            }
            return number;
        }

        private long GetPow(int count)
        {
            if (count == 0)
                return 1;
            return GetPow(count - 1) * _alphabet.Length;
        }
    }
}
