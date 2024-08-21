using Confluent.Kafka;
using MSMQ.Kafka.Factories;
using MSMQ.Common.Messages;

namespace MSMQ.Kafka.Services
{
    public interface IKafkaProducer
    {
        public Task Publish<T>(string topic, T payload, CancellationToken token);
        public Task Publish<T>(T payload, CancellationToken token);
    }

    public class KafkaProducer : IKafkaProducer
    {
        private readonly IKafkaProducerFactory _producerFactory;
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IKafkaTopicGenerator _kafkaTopicGenerator;

        public KafkaProducer(ILogger<KafkaProducer> logger, IKafkaTopicGenerator kafkaTopicGenerator, IKafkaProducerFactory producerFactory)
        {
            _logger = logger;
            _kafkaTopicGenerator = kafkaTopicGenerator;
            _producerFactory = producerFactory;
        }

        public async Task Publish<T>(string topic, T payload, CancellationToken token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(topic);
            ArgumentNullException.ThrowIfNull(payload);

            _logger.LogInformation("Starting message delivery to '{TopicName}' topic.", topic);
            try
            {
                var innerMessage = CommonMessage<T>.Create(payload);
                var message = new Message<Null, CommonMessage>
                {
                    Value = innerMessage,
                };

                using var producer = _producerFactory.Create();
                var result = await producer.ProduceAsync(topic, message, token);

                _logger.LogInformation("Message #{MessageId} delivered to '{DeliveryPartition}' successfully.", innerMessage.Id, result.TopicPartition);
            }
            catch (ProduceException<Null, CommonMessage> e)
            {
                _logger.LogError("Message was not deliveried to '{DeliveryPartition}' due to '{ErrorReason}'.", e.DeliveryResult.TopicPartition, e.DeliveryResult.Message);
                throw;
            }

        }

        public async Task Publish<T>(T payload, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(payload);

            try
            {
                var topic = _kafkaTopicGenerator.GetTopic(payload.GetType());
                _logger.LogInformation("Starting message delivery to '{TopicName}' topic.", topic);

                var innerMessage = CommonMessage<T>.Create(payload);
                var message = new Message<Null, CommonMessage>
                {
                    Value = innerMessage,
                };
                using var producer = _producerFactory.Create();
                var result = await producer.ProduceAsync(topic, message, token);

                _logger.LogInformation("Message #{MessageId} delivered to '{DeliveryPartition}' successfully.", innerMessage.Id, result.TopicPartition);
            }
            catch (ProduceException<Null, CommonMessage> e)
            {
                _logger.LogError("Message was not deliveried to '{DeliveryPartition}' due to '{ErrorReason}'.", e.DeliveryResult.TopicPartition, e.DeliveryResult.Message);
                throw;
            }
        }
    }
}
