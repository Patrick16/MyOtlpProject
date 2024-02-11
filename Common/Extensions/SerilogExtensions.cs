using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Enrichers.Span;

namespace Common.Extensions;

public static class SerilogExtensions
{
    public static IServiceCollection AddMySerilog(this IServiceCollection services, ResourceBuilder resourceBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithSpan()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Sink<CustomLogEventSink>()
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
            loggingBuilder.AddOpenTelemetry(options =>
                               options.SetResourceBuilder(resourceBuilder).IncludeScopes = true);
        });
        return services;
    }

    public static void RunWithLogging(this WebApplication app)
    {
        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
