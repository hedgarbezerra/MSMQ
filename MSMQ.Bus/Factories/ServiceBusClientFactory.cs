using Azure.Core.Pipeline;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Factories
{
    public interface IServiceBusClientFactory
    {
        ServiceBusClient Create();
        ServiceBusAdministrationClient CreateAdmin();
    }
    public class ServiceBusClientFactory(IConfiguration configuration) : IServiceBusClientFactory
    {
        public ServiceBusClient Create()
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                RetryOptions = new ServiceBusRetryOptions()
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    Delay = TimeSpan.FromSeconds(3),
                    MaxRetries = 3,
                    MaxDelay = TimeSpan.FromSeconds(9),
                }
            };
            var connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");
            var client = new ServiceBusClient(connectionString, clientOptions);

            return client;
        }

        public ServiceBusAdministrationClient CreateAdmin()
        {
            var connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");
            var client = new ServiceBusAdministrationClient(connectionString);

            return client;
        }
    }
}
