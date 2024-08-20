using MSMQ.Kafka.Exceptions;
using MSMQ.Kafka.Handlers;
using MSMQ.Kafka.Messages;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Factories
{
    public interface IKafkaConsumerHandlerFactory
    {
        List<IKafkaConsumerHandler> Create(KafkaMessage message);
    }

    public class KafkaConsumerHandlerFactory : IKafkaConsumerHandlerFactory
    {
        private readonly IKafkaTopicGenerator _kafkaTopicGenerator;
        private readonly ILogger<KafkaConsumerHandlerFactory> _logger;
        private readonly Dictionary<string, List<IKafkaConsumerHandler>> _topicServices;

        public KafkaConsumerHandlerFactory(IKafkaTopicGenerator kafkaTopicGenerator, ILogger<KafkaConsumerHandlerFactory> logger, IEnumerable<IKafkaConsumerHandler> consumerHandlers)
        {
            _kafkaTopicGenerator = kafkaTopicGenerator;
            _logger = logger;

            _topicServices = consumerHandlers.GroupBy(k => k.Topic)
                .ToDictionary(k => k.Key, g => g.ToList());
        }

        public List<IKafkaConsumerHandler> Create(KafkaMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            string topic = _kafkaTopicGenerator.GetTopic(message);

            if(!_topicServices.TryGetValue(topic, out List<IKafkaConsumerHandler> consumerHandlers))
                throw new ConsumerNotImplementedException(topic);

            return consumerHandlers;
        }
        //TODO: Criar método que retorne tópicos(string) registrados
    }
}
