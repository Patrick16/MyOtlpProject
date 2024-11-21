using Common.Sinks;
using Common.Telemetry.Processors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddMyOpenTelemetry(
        this IServiceCollection services, 
        string otlpUrl, 
        string serviceName,
        Func<HttpRequest, bool> httpRequestFilter = null)
    {     
        services.AddOpenTelemetry()
              .ConfigureResource(x => 
                x.AddService(serviceName))
              .WithTracing(tracing => tracing
                  .AddSource(serviceName)
                  .AddAspNetCoreInstrumentation(options =>
                  {
                      options.RecordException = true;
                      options.Filter = context =>
                      {
                          var path = context.Request.Path.ToString();
                          if (httpRequestFilter != null && httpRequestFilter(context.Request)) return false;
                          if (path.Contains("isalive")) return false;
                          if (path.Contains("metrics")) return false;
                          if (path.Contains("dependencies")) return false;
                          if (path.Contains("swagger")) return false;
                          if (path == "/") return false;
                          return true;
                      };
                  })
                  .AddEntityFrameworkCoreInstrumentation()
                  .AddProcessor<MySpanTraceProcessor>()
                  .AddProcessor<MyExceptionProcessor>()
                  .AddOtlpExporter(options =>
                  {
                      options.Endpoint = new Uri(otlpUrl);
                  }))
              .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(otlpUrl);
                }));
        return services;
    }
}
