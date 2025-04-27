using System.Reflection;
using Confluent.Kafka.Extensions.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MSMQ.Common.Extensions;

public static class WebBuilderExtensions
{
    public static WebApplicationBuilder AddMultiplayerOpenTelemetry(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        var service = configuration.GetValue<string>("ExecutingApp") ?? "MSMQ";
        var key = configuration.GetValue<string>("Multiplayer:Key") ?? throw new ArgumentNullException("Multiplayer:Key");
        var logsEndpoint = new Uri(configuration.GetValue<string>("Multiplayer:Endpoints:Logs"));
        var tracesEndpoint = new Uri(configuration.GetValue<string>("Multiplayer:Endpoints:Traces"));
        var metricsEndpoint = new Uri(configuration.GetValue<string>("Multiplayer:Endpoints:Metrics"));
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                        serviceName: service,
                        serviceNamespace: builder.Environment.EnvironmentName,
                        serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                        serviceInstanceId: Environment.MachineName
                    )
                    .AddAttributes(new Dictionary<string, object>
                    {
                        { "deployment.environment", builder.Environment.EnvironmentName },
                        { "service.name", service },
                    });
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments("/_health") &&
                            !context.Request.Path.StartsWithSegments("/_metrics");
                    })
                    .AddHttpClientInstrumentation()
                    .AddConfluentKafkaInstrumentation()
                    // .AddRabbitMQInstrumentation() instalar pacote RabbitMQ.Client.OpenTelemetry(prerealease)
                    .AddConsoleExporter()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = tracesEndpoint;
                        options.Headers = $"Authorization={key}";
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = metricsEndpoint;
                        options.Headers = $"Authorization={key}";
                    });
            })
            .WithLogging(logs => logs
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = logsEndpoint;
                    options.Headers = $"Authorization={key}";
                }));

        return builder;
    }
}