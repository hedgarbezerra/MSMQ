using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTelemetry(this IServiceCollection services, IConfiguration configurations)
        {

            services.AddOpenTelemetry()
                .ConfigureResource(r => r.AddService("MSMQ.Bus"))
                .WithTracing(tracing => tracing
                  .AddAspNetCoreInstrumentation()
                  .AddConsoleExporter())
              .WithMetrics(metrics => metrics
                  .AddAspNetCoreInstrumentation()
                  .AddConsoleExporter())
              .WithLogging(l => l.AddConsoleExporter());
            //services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            //{
            //    tracerProviderBuilder
            //        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyServiceName"))
            //        .AddAspNetCoreInstrumentation()
            //        .AddHttpClientInstrumentation()
            //        .AddSqlClientInstrumentation()
            //        .AddSeqExporter(options =>
            //        {
            //            options.ServerUrl = "http://localhost:5341"; // Replace with your Seq URL
            //            options.ApiKey = "your-seq-api-key"; // Replace with your Seq API key if needed
            //        });
            //});

            //// Optional: Configure OpenTelemetry metrics
            //services.AddOpenTelemetryMetrics(meterProviderBuilder =>
            //{
            //    meterProviderBuilder
            //        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyServiceName"))
            //        .AddAspNetCoreInstrumentation()
            //        .AddHttpClientInstrumentation();
            //    // Add other instrumentations as needed
            //});

            //// Optional: Configure OpenTelemetry logging
            //services.AddLogging(l => l.AddOpenTelemetry(options =>
            //{
            //    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyServiceName"));
            //    options.add
            //    options.AddSeq(options =>
            //    {
            //        options.ServerUrl = "http://localhost:5341"; // Replace with your Seq URL
            //        options.ApiKey = "your-seq-api-key"; // Replace with your Seq API key if needed
            //    });
            //}));
        }
    }
}
