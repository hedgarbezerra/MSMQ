using Azure.Messaging.ServiceBus;
using MSMQ.Bus.Services;
using MSMQ.Common.Serializers;

namespace MSMQ.Bus.Handlers
{
    public interface IMessageHandler
    {
        public string Queue { get; }
        Task Handle(ProcessMessageEventArgs @event, CancellationToken cancellationToken);
    }

    public abstract class MessageHandler<T> : IMessageHandler
    {
        protected readonly ILogger<MessageHandler<T>> _logger;
        protected readonly IServiceBusQueueGenerator _queueGenerator;
        protected readonly IMessagePublisher _messagePublisher;

        protected MessageHandler(ILogger<MessageHandler<T>> logger, IServiceBusQueueGenerator queueGenerator, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _queueGenerator = queueGenerator;
            _messagePublisher = messagePublisher;
        }

        public string Queue => _queueGenerator.GetQueue(typeof(T));

        public async Task Handle(ProcessMessageEventArgs @event, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var busMessage = @event.Message;
            var message = MessageSerializer.Deserialize(busMessage.Body);
            try
            {
                T payload = (T)message.Payload;

                _logger.LogInformation("Started consuming message #{MessageId} from queue '{MessageQueue}' by handler '{ConsumerHandler}'", message.Id, Queue, GetType().Name);

                await Handle(payload);
            }
            catch (Exception e)
            {
                _logger.LogError("Error occurred while consuming message #{MessageId} : {ErrorReason}", message.Id, e.Message);
            }
        }
        protected abstract Task Handle(T payload, CancellationToken cancellationToken = default);
    }

}
