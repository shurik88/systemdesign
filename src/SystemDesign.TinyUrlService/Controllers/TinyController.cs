using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SystemDesign.TinyUrlService.Services;

namespace SystemDesign.TinyUrlService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinyController : ControllerBase
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<TinyUrl> _tinies;
        private readonly TinyUrlGenerator _tinyGenerator;
        public TinyController(IMongoDatabase database, TinyUrlGenerator tinyGenerator)
        {
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _tinies = _db.GetCollection<TinyUrl>("tinies");
            _tinyGenerator = tinyGenerator ?? throw new ArgumentNullException(nameof(tinyGenerator));
        }


        /// <summary>
        ///     Генерация короткой ссылки.
        /// </summary>
        /// <param name="data">Данные для генерации</param>
        /// <returns>Короткая строка</returns>
        [Produces(typeof(string))]
        [HttpPost(Name = "PostUrl")]
        public async Task<IActionResult> PostUrlAsync([FromBody] TinyUrlDto data, [FromServices] SnowflakeGenerator.SnowflakeGenerator snowflakeGenerator)
        {
            if (data == null)
                return BadRequest("empty model");

            if (await _tinies.AsQueryable().AnyAsync(x => x.Url == data.Url))
                return Conflict("Tiny url already exists");

            var baseNumber = snowflakeGenerator.GetId();
            var tiny = _tinyGenerator.Generate(baseNumber);

            await _tinies.InsertOneAsync(new TinyUrl { BaseNumber = baseNumber, Created = DateTime.UtcNow, Id = Guid.NewGuid(), Tiny = tiny, Url = data.Url });

            return Ok(tiny);
        }

        /// <summary>
        ///    Переход по ссылке.
        /// </summary>
        /// <param name="url">Короткая ссылка</param>
        /// <returns>Исходный адрес</returns>
        [HttpGet("{url}")]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(301)]
        public async Task<IActionResult> GetUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("empty url");

            var baseNumber = _tinyGenerator.GetBaseNumberFromString(url);
            if (baseNumber == null)
                return BadRequest("invalid tiny url");

            var tiny = await _tinies.AsQueryable().Where(x => x.BaseNumber == baseNumber).Select(x => new { Url = x.Url}).FirstOrDefaultAsync();
            if (tiny == null)
                return NotFound($"url:{url}");

            return RedirectPermanent(tiny.Url);
        }


    }
}
