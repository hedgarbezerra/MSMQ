using MSMQ.Kafka.Messages;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Handlers
{
    public interface IKafkaConsumerHandler
    {
        public string Topic { get; }
        Task Handle(KafkaMessage message, CancellationToken cancellationToken = default);
    }

    public abstract class KafkaConsumerHandler<T> : IKafkaConsumerHandler
    {
        protected readonly ILogger<KafkaConsumerHandler<T>> _logger;
        protected readonly IKafkaProducer _producer;
        private readonly IKafkaTopicGenerator _kafkaTopicGenerator;

        protected KafkaConsumerHandler(ILogger<KafkaConsumerHandler<T>> logger, IKafkaProducer producer, IKafkaTopicGenerator kafkaTopicGenerator)
        {
            _logger = logger;
            _producer = producer;
            _kafkaTopicGenerator = kafkaTopicGenerator;
        }

        public string Topic => _kafkaTopicGenerator.GetTopic(typeof(T));

        public async Task Handle(KafkaMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);
            try
            {
                _logger.LogInformation("Started consuming message #{MessageId} from topic '{MessageTopic}' by handler '{ConsumerHandler}'", message.Id, Topic, this.GetType().Name);
                var payload = (T)message.Payload;

                await Handle(message.Id, payload, cancellationToken);

                _logger.LogInformation("Message #{MessageId} has been consumed by handler '{ConsumerHandler}'", message.Id, this.GetType().Name);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occurred while consuming message #{MessageId} : {ErrorReason}", message.Id, ex.Message);
            }
        }

        protected abstract Task Handle(Guid sourceId, T payload, CancellationToken cancellationToken = default);

        protected KafkaMessage<TResult> PackMessage<TResult>(TResult result) => KafkaMessage<TResult>.Create(result);
    }
}
