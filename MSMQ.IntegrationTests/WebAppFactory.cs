using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSMQ.API;
using MSMQ.Common;
using MSMQ.Kafka;
using MSMQ.RabbitMQ;
using OpenTelemetry.Trace;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.Kafka;
using Testcontainers.RabbitMq;

namespace MSMQ.IntegrationTests
{
    public class WebAppFactory : WebApplicationFactory<API.Program>
    {
        private readonly KafkaContainer _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithEnvironment("KAFKA_BROKER_ID", "1")
            .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
            .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_MIN_ISR", "1")
            .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR", "1")
            .Build();

        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .Build();

        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            var inMemorySettings = new Dictionary<string, string>
                {
                    { "Logging:LogLevel:Default", "Information" },
                    { "Logging:LogLevel:Microsoft.AspNetCore", "Warning" },
                    { "Tasks:IpAddressUpdater:RepeatEveryMinutes", "60" },
                    { "API:Version", "1" },
                    { "ExecutingApp", "MSMQ-WebAPI" },
                    { "RabbitMq:Url", _rabbitMqContainer.GetConnectionString() },
                    { "Kafka:GroupId", "groupid-msmq" },
                    { "Kafka:BootstrapServers", _kafkaContainer.GetBootstrapAddress() },
                    { "AllowedHosts", "*" }
                };
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings).Build();
            builder.UseConfiguration(Configuration);

            builder.ConfigureTestServices(services =>
            {
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddKafkaServices(Configuration);
                services.AddRabbitMqServices(Configuration);

                Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Console(Serilog.Events.LogEventLevel.Information)              
               .CreateLogger();

                services.AddSerilog();
            });
        }

        public async Task StartContainers()
        {
            await _kafkaContainer.StartAsync();
            await _rabbitMqContainer.StartAsync();
        }
        public async Task StopContainers()
        {
            await _kafkaContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
        }
    }
}
