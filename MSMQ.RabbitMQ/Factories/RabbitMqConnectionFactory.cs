using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Factories
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection Create();
    }
    public class RabbitMqConnectionFactory(IConfiguration _configuration) : IRabbitMqConnectionFactory
    {
        public IConnection Create()
        {
            //testar URI
            var factory = new ConnectionFactory()
            {
                UserName = _configuration.GetValue<string>("RabbitMq:Username"),
                Password = _configuration.GetValue<string>("RabbitMq:Password"),
                HostName = _configuration.GetValue<string>("RabbitMq:HostName"),
                VirtualHost = _configuration.GetValue<string>("RabbitMq:VirtualHost"),
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }
    }
}
