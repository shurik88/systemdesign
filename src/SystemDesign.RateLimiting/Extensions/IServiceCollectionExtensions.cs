using SystemDesign.RateLimiting.RateLimit;
using SystemDesign.RateLimiting.Redis;

namespace SystemDesign.RateLimiting.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisRateLimiter(this IServiceCollection services, string redisConnectionString)
        {
            return services.AddRedis(redisConnectionString)
                .AddScoped<IRateLimiter, RedisRateLimiter>()
                .AddScoped<UserRateLimitResourceFilter>();
        }

        private static IServiceCollection AddRedis(this IServiceCollection services, string redisConnectionString)
        {
            return services.AddSingleton(RedisConnection.Init(redisConnectionString));
        }
    }
}
