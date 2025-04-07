using AspNetCoreRateLimit;
using System.Net;

namespace backend.Infraestructure
{
    public static class RateLimitConfig
    {
        public static void AddRateLimiting(this IServiceCollection services)
        {
            services.Configure<IpRateLimitOptions>(options =>
           {
               options.EnableEndpointRateLimiting = true;
               options.StackBlockedRequests = false;
               options.GeneralRules = new List<RateLimitRule>
               {
                    new() {
                        Endpoint = "*",
                        Period = "1m",
                        Limit = 20
                    }
               };
               options.QuotaExceededResponse = new QuotaExceededResponse
               {
                   Content = "Número máximo de llamadas por minuto alcanzado.",
                   StatusCode = (int)HttpStatusCode.TooManyRequests
               };
           });
        }
    }
}
