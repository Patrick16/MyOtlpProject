using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddMyOpenTelemetry(this IServiceCollection services, string serviceName)
    {
        services.AddOpenTelemetry()
              .ConfigureResource(x => x.AddService(serviceName))
              .WithTracing(tracing => tracing
                  .AddAspNetCoreInstrumentation()
                  .AddSource(serviceName)
                  //.AddEntityFrameworkCoreInstrumentation()
                  .AddOtlpExporter(options =>
                  {
                      options.Endpoint = new Uri("http://localhost:4317");
                  }))
              .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:4317");
                }));
        return services;
    }
}
