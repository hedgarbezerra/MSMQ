using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using MSMQ.Kafka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.IntegrationTests.Kafka
{
    internal static class KafkaTopicsInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, out List<TopicSpecification> topics) 
        {
            var kafkaTopicsGenerator = serviceProvider.GetRequiredService<IKafkaTopicGenerator>();
            var kafkaTopicsCreator = serviceProvider.GetRequiredService<IKafkaTopicCreator>();

            topics = kafkaTopicsGenerator.GetTopics();

            kafkaTopicsCreator.Create(topics.ToArray()).Wait();
        }
    }
}
