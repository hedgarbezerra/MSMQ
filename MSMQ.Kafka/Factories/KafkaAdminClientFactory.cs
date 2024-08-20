using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.Kafka.Factories
{
    public interface IKafkaAdminClientFactory
    {
        public IAdminClient Create();
    }

    public class KafkaAdminClientFactory : IKafkaAdminClientFactory
    {
        private readonly IConfiguration _configuration;

        public KafkaAdminClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAdminClient Create()
        {
            var configurations = new AdminClientConfig()
            {
                BootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers"),
            };

            return new AdminClientBuilder(configurations).Build();
        }
    }
}
