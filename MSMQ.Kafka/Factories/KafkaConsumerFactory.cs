using Confluent.Kafka;
using MSMQ.Kafka.Messages;
using MSMQ.Kafka.Services;

namespace MSMQ.Kafka.Factories
{
    public interface IKafkaConsumerFactory
    {
        public IConsumer<Null, KafkaMessage> Create();
    }
    public class KafkaConsumerFactory : IKafkaConsumerFactory
    {
        private const int MAX_ACCEPTED_BYTES = 1024 * 1024 * 10;
        private readonly IConfiguration _configuration;

        public KafkaConsumerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConsumer<Null, KafkaMessage> Create()
        {
            var config = new ConsumerConfig()
            {
                AllowAutoCreateTopics = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers"),
                GroupId = _configuration.GetValue<string>("Kafka:GroupId"),
                MessageMaxBytes = MAX_ACCEPTED_BYTES,
                MaxPartitionFetchBytes = MAX_ACCEPTED_BYTES,
            };

            var builder = new ConsumerBuilder<Null, KafkaMessage>(config)
                .SetValueDeserializer(new KafkaMessageSerializer());

            return builder.Build();
        }
    }
}
