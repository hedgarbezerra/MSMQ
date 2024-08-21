using Confluent.Kafka;
using MSMQ.Common.Messages;
using MSMQ.Kafka.Services;

namespace MSMQ.Kafka.Factories
{
    public interface IKafkaProducerFactory
    {
        public IProducer<Null, CommonMessage> Create();
    }
    public class KafkaProducerFactory : IKafkaProducerFactory
    {
        private const int MAX_ACCEPTED_BYTES = 1024 * 1024 * 10;
        private readonly IConfiguration _configuration;

        public KafkaProducerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IProducer<Null, CommonMessage> Create()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers"),
                MessageMaxBytes = MAX_ACCEPTED_BYTES,
                MessageTimeoutMs = 100_000
            };

            var builder = new ProducerBuilder<Null, CommonMessage>(config)
                .SetValueSerializer(new KafkaMessageSerializer());

            return builder.Build();
        }
    }
}
