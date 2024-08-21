using Confluent.Kafka.Admin;
using MSMQ.Kafka.Handlers;
using MSMQ.Common.Messages;

namespace MSMQ.Kafka.Services
{
    public interface IKafkaTopicGenerator
    {
        public string GetTopic(Type payloadType);
        string GetTopic(CommonMessage message);
        List<TopicSpecification> GetTopics();
    }

    public class KafkaTopicGenerator : IKafkaTopicGenerator
    {
        public List<TopicSpecification> GetTopics()
        {
            var expectedType = typeof(IKafkaConsumerHandler);

            var implementations = expectedType
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && expectedType.IsAssignableFrom(t))
                .ToList();
            
            //Must have base class and a single generic parameter
            var impGenericParameters = implementations.Select(i => i.BaseType.GenericTypeArguments.First());

            var topicNames = impGenericParameters
                .Select(t => $"topic-{t.FullName.ToLower().Trim()}")
                .Distinct();

            var topics = topicNames.Select(t => new TopicSpecification() { Name = t, NumPartitions = 1 })
                .ToList();

            return topics;
        }

        public string GetTopic(CommonMessage message)
        {
            var payloadType = message.GetPayloadType();

            return GetTopic(payloadType);
        }

        public string GetTopic(Type payloadType) => $"topic-{payloadType?.FullName?.ToLower().Trim()}";
    }
}
