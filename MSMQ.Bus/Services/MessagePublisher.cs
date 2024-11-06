using Azure.Messaging.ServiceBus;
using MSMQ.Common.Messages;
using MSMQ.Common.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Bus.Services
{
    public interface IMessagePublisher
    {
        Task Publish(CommonMessage message, CancellationToken cancellationToken = default);
        Task Publish<T>(T payload, CancellationToken cancellationToken = default);
    }

    public class MessagePublisher : IMessagePublisher
    {
        private readonly ILogger<MessagePublisher> _logger;
        private readonly IServiceBusQueueGenerator _queueGenerator;
        private readonly Dictionary<string, ServiceBusSender> _senders;

        public MessagePublisher(ILogger<MessagePublisher> logger, IServiceBusQueueGenerator queueGenerator, ServiceBusClient client)
        {
            _logger = logger;
            _queueGenerator = queueGenerator;
            _senders = queueGenerator.GetQueues()
                .ToDictionary(q => q, q => client.CreateSender(q));
        }

        public async Task Publish<T>(T payload, CancellationToken cancellationToken = default)
        {
            var message = CommonMessage<T>.Create(payload);
            await PublishInternal(message, cancellationToken);
        }
        public async Task Publish(CommonMessage message, CancellationToken cancellationToken = default) =>
            await PublishInternal(message, cancellationToken);

        private async Task PublishInternal(CommonMessage message, CancellationToken cancellationToken = default)
        {
            string queueName = _queueGenerator.GetQueue(message);
            try
            {
                var messageBinary = MessageSerializer.Serialize(message);
                var busMessage = new ServiceBusMessage(messageBinary)
                {
                    MessageId = message.Id.ToString()
                };

                if (!_senders.TryGetValue(queueName, out var publisher))
                    throw new InvalidOperationException($"Queue message publisher for '{queueName} not implemented.'");

                await publisher.SendMessageAsync(busMessage, cancellationToken);
                _logger.LogInformation("Message published to Service Bus' queue '{QueueName}'", queueName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while publishing message to Service Bus' queue '{QueueName}'", queueName);
            }

        }
    }
}
