using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;
using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Exporter;

namespace MSMQ.Bus
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            Uri logsEndpoint = new Uri(configuration.GetValue<string>("Seq:Endpoints:Logs"));
            Uri tracesEndpoint = new Uri(configuration.GetValue<string>("Seq:Endpoints:Traces"));

            ResourceBuilder resource = ResourceBuilder.CreateDefault()
                .AddService("MSMQ")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["environment"] = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>().EnvironmentName,
                    ["service.name"] = "MSMQ"
                });

            services.AddOpenTelemetry()
                .ConfigureResource(r => r = resource)
                .WithTracing(tracing => tracing
                    .AddSource("Example.Source")
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = tracesEndpoint;
                        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                        exporter.Headers = $"X-Seq-ApiKey={configuration.GetValue<string>("Seq:Key")}";
                    }));

            services.AddLogging(builder =>
                builder.AddOpenTelemetry(otlpOptions =>
                {
                    otlpOptions.IncludeFormattedMessage = true;
                    otlpOptions.IncludeScopes = true;
                    otlpOptions.AddConsoleExporter();
                    otlpOptions.AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = logsEndpoint;
                        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                        exporter.Headers = $"X-Seq-ApiKey={configuration.GetValue<string>("Seq:Key")}";
                    })
                    .SetResourceBuilder(resource);
                })
            );

            //se faz necessário o pacote 'Seq.Extensions.Logging'
            //services.AddLogging(l => l.AddOpenTelemetry(options =>
            //{
            //    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MSMQ"));

            //    options.AddSeq(options =>
            //    {
            //        options.ServerUrl = "http://localhost:5341"; // Replace with your Seq URL
            //        options.ApiKey = "your-seq-api-key"; // Replace with your Seq API key if needed
            //    });
            //}));
        }

        public static void AddSerilogOptl(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .WriteTo.OpenTelemetry(opt =>
                    {
                        opt.Endpoint = configuration.GetValue<string>("Seq:Endpoints:Root");
                        opt.LogsEndpoint = configuration.GetValue<string>("Seq:Endpoints:Logs");
                        opt.TracesEndpoint = configuration.GetValue<string>("Seq:Endpoints:Traces");
                        opt.IncludedData = IncludedData.TraceIdField | IncludedData.SourceContextAttribute | IncludedData.SpanIdField | IncludedData.TemplateBody;
                        opt.Protocol = OtlpProtocol.HttpProtobuf;
                        opt.Headers = new Dictionary<string, string>()
                        {
                            ["X-Seq-ApiKey"] = configuration.GetValue<string>("Seq:Key")
                        };
                        opt.ResourceAttributes = new Dictionary<string, object>()
                        {
                            ["service.name"] = "MSMQ"
                        };
                    })
                .CreateLogger();

            //Deve adicionar para tracing
            services.AddOpenTelemetry()
               .WithTracing(tracing => tracing
                   .AddSource("Example.Source")
                   .AddAspNetCoreInstrumentation()
                   .AddConsoleExporter()
                   .AddOtlpExporter(exporter =>
                   {
                       exporter.Endpoint = new Uri(configuration.GetValue<string>("Seq:Endpoints:Traces"));
                       exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                       exporter.Headers = $"X-Seq-ApiKey={configuration.GetValue<string>("Seq:Key")}";
                   }));

            services.AddSerilog();
        }

        public static void AddSerilogSeq(this IServiceCollection services, IConfiguration configuration)
        {
            //Quando apontar para o Seq, não é necessário especificar o endpoint exato do ingest. Já faz trace automático.
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .WriteTo.Seq(configuration.GetValue<string>("Seq:Endpoints:Root"),
                    apiKey: configuration.GetValue<string>("Seq:Key"),
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            //Deve adicionar para tracing
            services.AddOpenTelemetry()
               .WithTracing(tracing => tracing
                   .AddSource("Example.Source")
                   .AddAspNetCoreInstrumentation()
                   .AddConsoleExporter()
                   .AddOtlpExporter(exporter =>
                   {
                       exporter.Endpoint = new Uri(configuration.GetValue<string>("Seq:Endpoints:Traces"));
                       exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                       exporter.Headers = $"X-Seq-ApiKey={configuration.GetValue<string>("Seq:Key")}";
                   }));

            services.AddSerilog();
        }
    }
}
