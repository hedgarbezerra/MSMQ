using Confluent.Kafka;
using MSMQ.Kafka.Exceptions;
using MSMQ.Kafka.Factories;
using MSMQ.Kafka.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Services
{
    public interface IKafkaRunningService
    {
        Task Run(CancellationToken cancellationToken);
    }

    public class KafkaRunningService : IKafkaRunningService
    {
        private readonly ILogger<KafkaRunningService> _logger;
        private readonly IKafkaConsumerFactory _consumerFactory;
        private readonly IKafkaTopicGenerator _topicGenerator;
        private readonly IKafkaTopicCreator _topicCreator;
        private readonly IKafkaConsumerHandlerFactory _consumerHandlerFactory;

        public KafkaRunningService(ILogger<KafkaRunningService> logger, IKafkaConsumerFactory consumerFactory, IKafkaTopicGenerator topicGenerator, IKafkaTopicCreator topicCreator, IKafkaConsumerHandlerFactory consumerHandlerFactory)
        {
            _logger = logger;
            _consumerFactory = consumerFactory;
            _topicGenerator = topicGenerator;
            _topicCreator = topicCreator;
            _consumerHandlerFactory = consumerHandlerFactory;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting services...");

            var consumer = _consumerFactory.Create();
            var topics = _topicGenerator.GetTopics();

            await _topicCreator.Create(topics.ToArray());

            consumer.Subscribe(topics.Select(t => t.Name));

            foreach (var topic in topics)
                _logger.LogInformation("Consumer has subscribed to the following topic: '{TopicName}'", topic.Name);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Consumer '{ConsumerName}' is waiting for messages...", consumer.Name);

                    var consumeResult = consumer.Consume(cancellationToken);
                    await ConsumeHandler(consumeResult, cancellationToken);
                }
            }
            catch (OperationCanceledException e)
            {
                _logger.LogInformation("Operation was cancelled");
            }
            catch (KafkaException e)
            {
                _logger.LogError("Error: '{ErrorMessage}' ocurred due to '{ErrorReason}' ", e.Message, e.Error.Reason);
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("Stopping service and dependencies...");
            }
        }

        private async Task ConsumeHandler(ConsumeResult<Null, KafkaMessage> result, CancellationToken cancellationToken = default)
        {
            if (result is null)
                throw new InvalidOperationException("Invalid operation due to consume result being null.");

            try
            {
                var message = result.Message.Value;
                var messageConsumers = _consumerHandlerFactory.Create(message);

                foreach (var handler in messageConsumers)
                {
                    await handler.Handle(message, cancellationToken);
                }
            }
            catch(ConsumerNotImplementedException e)
            {
                _logger.LogError("Message #{MessageId} for topic '{MessageTopic}' does not have consumer handler implemented or was not identified.", e.Message, e.Topic);
            }
        }
    }
}
