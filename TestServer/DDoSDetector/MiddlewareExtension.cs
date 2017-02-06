using Microsoft.AspNetCore.Builder;

namespace DDoSDetector
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseDosProtection(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<Detector>();
        }
    }
}