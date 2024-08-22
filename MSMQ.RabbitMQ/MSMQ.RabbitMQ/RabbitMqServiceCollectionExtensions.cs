using MSMQ.RabbitMQ.Consumers;
using MSMQ.RabbitMQ.Factories;
using MSMQ.RabbitMQ.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ
{
    public static class RabbitMqServiceCollectionExtensions
    {
        public static void AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IRabbitMqConnectionFactory>();

                return factory.Create();
            });

            services.AddScoped<IRabbitExchangeCreator, RabbitExchangeCreator>();
            services.AddScoped<IRabbitQueueCreator, RabbitQueueCreator>();
            services.AddScoped<IRabbitQueuesGenerator, RabbitQueuesGenerator>();
            services.AddScoped<IRabbitMessagesQueueCreator, RabbitMessagesQueueCreator>();
            services.AddScoped<IRabbitConsumerFactory, RabbitConsumerFactory>();
            services.AddScoped<IRabbitProducer, RabbitProducer>();
            services.AddScoped<IRabbitMqExecutingService, RabbitMqExecutingService>();

            services.AddScoped<IRabbitConsumer, MovieAddedConsumer>();
            services.AddScoped<IRabbitConsumer, MovieRemovedConsumer>();
            services.AddScoped<IRabbitConsumer, MovieRemovedConsumer2>();
            services.AddScoped<IRabbitConsumer, MovieUpdatedConsumer>();
            services.AddScoped<IRabbitConsumer, MovieRetrievedConsumer>();
        }
    }
}
