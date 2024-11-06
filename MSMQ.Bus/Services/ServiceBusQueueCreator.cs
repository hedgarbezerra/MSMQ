using Azure.Messaging.ServiceBus.Administration;
using MSMQ.Bus.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Services
{
    public interface IServiceBusQueueCreator
    {
        Task CreateQueue(string queueName, CancellationToken cancellationToken = default);
        Task CreateQueues(List<string> queues, CancellationToken cancellationToken = default);
    }

    public class ServiceBusQueueCreator : IServiceBusQueueCreator
    {
        private readonly ILogger<ServiceBusQueueCreator> _logger;
        private readonly ServiceBusAdministrationClient _client;

        public ServiceBusQueueCreator(ILogger<ServiceBusQueueCreator> logger, ServiceBusAdministrationClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task CreateQueue(string queueName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _client.QueueExistsAsync(queueName, cancellationToken))
                {
                    var queueOptions = new CreateQueueOptions(queueName)
                    {
                        MaxSizeInMegabytes = 1024,
                        DefaultMessageTimeToLive = TimeSpan.FromDays(3),
                        DeadLetteringOnMessageExpiration = true,
                    };


                    await _client.CreateQueueAsync(queueOptions);
                    _logger.LogInformation("Queue '{queueName}' created successfully.", queueName);
                }
                else
                {
                    _logger.LogWarning("Queue '{queueName}' already exists, skipping creation.", queueName);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while creating new queue '{QueueName}': {ErrorReason}", queueName, e.Message);
            }
        }

        public async Task CreateQueues(List<string> queues, CancellationToken cancellationToken = default)
        {
            foreach (var queue in queues)
                await CreateQueue(queue, cancellationToken);
        }
    }
}
