using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddMyOpenTelemetry(this IServiceCollection services, string otlpUrl, string serviceName)
    {
        services.AddOpenTelemetry()
              .ConfigureResource(x => x.AddService(serviceName))
              .WithTracing(tracing => tracing
                  .AddSource(serviceName)
                  .AddAspNetCoreInstrumentation()
                  .AddEntityFrameworkCoreInstrumentation()
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
