
using Microsoft.Extensions.DependencyInjection.Extensions;
using SystemDesign.RateLimiting.Extensions;
using SystemDesign.RateLimiting.Impls;
using SystemDesign.RateLimiting.RateLimit;

namespace SystemDesign.RateLimiting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var redisCs = builder.Configuration.GetConnectionString("redis");
            // Add services to the container.

            builder.Services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IUserIdGeter, HttpHeaderUserIdGeter>()
                .AddRedisRateLimiter(redisCs);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(UserRateLimitResourceFilter));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
