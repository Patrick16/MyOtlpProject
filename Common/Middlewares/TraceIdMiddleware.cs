using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Common.Middlewares;

public class TraceIdMiddleware
{
    private readonly RequestDelegate _next;
    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = Activity.Current ?? new Activity("Service");
        var traceId = activity.TraceId.ToString();
        context.Response.OnStarting(state =>
        {
            var response = (HttpResponse)state;
            response.Headers.Append("trace-id", traceId);
            return Task.CompletedTask;
        }, context.Response);

        await _next(context);
    }
}
