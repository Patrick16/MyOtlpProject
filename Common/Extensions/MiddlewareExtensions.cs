using Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseTraceId(this IApplicationBuilder app)
        => app.UseMiddleware<TraceIdMiddleware>();
}
