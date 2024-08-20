using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MSMQ.Kafka.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Services
{
    public interface IKafkaTopicCreator
    {
        public Task Create(params TopicSpecification[] topics);
    }

    public class KafkaTopicCreator : IKafkaTopicCreator
    {
        private readonly IKafkaAdminClientFactory _adminClientFactory;
        private readonly ILogger<KafkaTopicCreator> _logger;

        public KafkaTopicCreator(IKafkaAdminClientFactory adminClientFactory, ILogger<KafkaTopicCreator> logger)
        {
            _adminClientFactory = adminClientFactory;
            _logger = logger;
        }

        public async Task Create(params TopicSpecification[] topics)
        {
            using(var client = _adminClientFactory.Create())
            {
                _logger.LogInformation("Creating {TopicsCount} topics if not created, otherwise follow program.", topics.Length);
                try
                {
                    await client.CreateTopicsAsync(topics);
                }
                catch (CreateTopicsException e)
                {
                    foreach (var result in e.Results)
                        if (result.Error.Code != ErrorCode.TopicAlreadyExists)
                            _logger.LogError("Error occurred creating topic '{TopicName}' due to '{ErrorReason}'", result.Topic, e.Error.Reason);

                }
            }
        }
    }
}
