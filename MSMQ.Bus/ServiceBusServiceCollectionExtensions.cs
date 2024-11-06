using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Azure;
using MSMQ.Bus.Factories;
using MSMQ.Bus.Handlers;
using MSMQ.Bus.Services;

namespace MSMQ.Bus
{
    public static class ServiceBusServiceCollectionExtensions
    {

        public static void AddServiceBusAzExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceBusServices(configuration);
            services.AddAzureClients(builder =>
            {
                string connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");

                builder.AddServiceBusClient(connectionString);
                builder.AddServiceBusAdministrationClient(connectionString);
            });
        }

        public static void AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceBusServices(configuration);
            services.AddSingleton<ServiceBusClient>(sp =>
            {
                var factory = sp.GetRequiredService<IServiceBusClientFactory>();

                return factory.Create();
            });
            services.AddSingleton<ServiceBusAdministrationClient>(sp =>
            {
                var factory = sp.GetRequiredService<IServiceBusClientFactory>();

                return factory.CreateAdmin();
            });
        }

        private static void AddServiceBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>();
            services.AddScoped<IMessageHandlersFactory, MessageHandlersFactory>();

            services.AddScoped<IMessageHandler, AddMovieMessageHandler>();
            services.AddScoped<IMessageHandler, MovieAddedEventHandler>();
            services.AddScoped<IMessageHandler, RemoveMovieMovieHandler>();

            services.AddScoped<IServiceBusQueueGenerator, ServiceBusQueueGenerator>();
            services.AddScoped<IServiceBusQueueCreator, ServiceBusQueueCreator>();
            services.AddScoped<IMessagePublisher, MessagePublisher>();
            services.AddScoped<IQueueSubscriber, QueueSubscriber>();

            services.AddScoped<IServiceBusRunningService, ServiceBusRunningService>();
        }
    }
}
