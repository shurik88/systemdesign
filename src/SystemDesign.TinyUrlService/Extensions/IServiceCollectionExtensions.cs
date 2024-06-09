using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver;
using SystemDesign.TinyUrlService.Services;
using SystemDesign.TinyUrlService.Settings;
using SystemDesign.TinyUrlService.Validators;

namespace SystemDesign.TinyUrlService.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public  static IServiceCollection AddTinyUrls(this IServiceCollection services, string mongoConnectionString, TinyUrlSettings settings)
        {
            var mongoUrl = new MongoUrl(mongoConnectionString);

            var mongoSettings = MongoClientSettings.FromUrl(mongoUrl);
            //if (isDevelopment)
            //{
            //    settings.ClusterConfigurator = cb =>
            //    {
            //        cb.Subscribe<CommandStartedEvent>(e =>
            //        {
            //            Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            //        });
            //    };
            //}
            var mongoClient = new MongoClient(mongoSettings);
            var db = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            return services.AddSingleton(mongoClient)
                .AddSingleton(db)
                .AddSingleton(settings)
                .AddTransient<IValidator<TinyUrlDto>, TinyUrlDtoValidator>()
                .AddSingleton(new SnowflakeGenerator.SnowflakeGenerator(settings.Snowflake.Datacenter, settings.Snowflake.Worker))
                .AddSingleton(new TinyUrlGenerator(settings.Alphabet))
                .AddFluentValidationAutoValidation();
        }
    }
}
