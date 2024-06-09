using MongoDB.Bson.Serialization.Attributes;

namespace SystemDesign.TinyUrlService
{
    /// <summary>
    ///     Ссылка.
    /// </summary>
    public class TinyUrl
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; }

        /// <summary>
        ///     Базовое число.
        /// </summary>
        [BsonElement("num")]
        public long BaseNumber { get; set; }

        /// <summary>
        ///     Исходный адрес.
        /// </summary>
        [BsonElement("url")]
        public string Url { get; set; }

        /// <summary>
        ///     Сокращенный.
        /// </summary>
        [BsonElement("tiny")]
        public string Tiny { get; set; }

        /// <summary>
        ///     Дата создания.
        /// </summary>
        [BsonElement("dt")]
        public DateTime Created { get; set; }
    }
}
