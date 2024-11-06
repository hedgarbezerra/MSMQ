using Confluent.Kafka;
using MSMQ.Common.Messages;
using MSMQ.Kafka.Services;

namespace MSMQ.Kafka.Factories
{
    public interface IKafkaConsumerFactory
    {
        public IConsumer<Null, CommonMessage> Create();
    }
    public class KafkaConsumerFactory : IKafkaConsumerFactory
    {
        private const int MAX_ACCEPTED_BYTES = 1024 * 1024 * 10;
        private readonly IConfiguration _configuration;

        public KafkaConsumerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConsumer<Null, CommonMessage> Create()
        {
            var config = new ConsumerConfig()
            {
                AllowAutoCreateTopics = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers"),
                GroupId = _configuration.GetValue<string>("Kafka:GroupId"),
                MessageMaxBytes = MAX_ACCEPTED_BYTES,
                MaxPartitionFetchBytes = MAX_ACCEPTED_BYTES,
                EnableAutoCommit = false, //Garantir o consumo antes de commitar(permite que a mensage mseja consumida por outro consumidor
            };

            var builder = new ConsumerBuilder<Null, CommonMessage>(config)
                .SetValueDeserializer(new KafkaMessageSerializer());

            return builder.Build();
        }
    }
}
