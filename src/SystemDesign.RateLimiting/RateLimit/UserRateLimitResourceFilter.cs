using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace SystemDesign.RateLimiting.RateLimit
{
    public class UserRateLimitResourceFilter : IAsyncResourceFilter
    {
        private static UserRateLimitAttribute ReadAttribute(ResourceExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
                return (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes<UserRateLimitAttribute>().SingleOrDefault();
            else return null;
        }

        /// <inheritdoc/>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var rateLimitAttr = ReadAttribute(context);

            if (rateLimitAttr == null)
            {
                await next();
                return;
            }

            var userIdGeter = context.HttpContext.RequestServices.GetRequiredService<IUserIdGeter>();

            var userId = userIdGeter.Id;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result =new ObjectResult("Required user Id")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                var rateLimiter = context.HttpContext.RequestServices.GetRequiredService<IRateLimiter>();
                var result = await rateLimiter.TryDoActionAsync(userId, new RateLimiterAction { Operation = rateLimitAttr.Action, Interval = rateLimitAttr.Interval, Total = rateLimitAttr.Total });
                if (result.IsSuccess)
                {
                    context.HttpContext.Response.Headers.Append("X-RateLimit-Remaining", result.Remain.ToString());
                    context.HttpContext.Response.Headers.Append("X-RateLimit-Limit", result.Total.ToString());
                    await next();
                }
                else
                {
                    context.Result = new StatusCodeResult(429);
                    context.HttpContext.Response.Headers.Append("X-RateLimit-Remaining", "0");
                    context.HttpContext.Response.Headers.Append("X-RateLimit-Limit", result.Total.ToString());
                    context.HttpContext.Response.Headers.Append("Retry-After", result.After.ToString());
                }
            }
        }
    }
}
