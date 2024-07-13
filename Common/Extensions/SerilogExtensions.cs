using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.OpenTelemetry;

namespace Common.Extensions;

public static class SerilogExtensions
{
    public static IServiceCollection AddMySerilog(this IServiceCollection services, string endpoint, string resourceName)
    {
        ResourceBuilder resource = ResourceBuilder.CreateDefault().AddService(resourceName);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithSpan()
            .ReadFrom.Configuration(configuration)
            //.WriteTo.Sink<CustomLogEventSink>()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = endpoint;
                options.IncludedData = 
                    IncludedData.TraceIdField | 
                    IncludedData.SpanIdField |
                    IncludedData.TemplateBody;
            }).CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
            loggingBuilder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resource).IncludeScopes = true;
               // options.AddProcessor(new LogProcessor());
            });
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
