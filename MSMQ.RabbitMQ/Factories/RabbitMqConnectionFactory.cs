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
            var factory = _configuration.GetValue<string>("RabbitMq:Url") switch
            {
                { Length: > 0 } => new ConnectionFactory() { Uri = new Uri(_configuration.GetValue<string>("RabbitMq:Url")), DispatchConsumersAsync = true },
                "" => new ConnectionFactory()
                {
                    UserName = _configuration.GetValue<string>("RabbitMq:Username"),
                    Password = _configuration.GetValue<string>("RabbitMq:Password"),
                    HostName = _configuration.GetValue<string>("RabbitMq:HostName"),
                    VirtualHost = _configuration.GetValue<string>("RabbitMq:VirtualHost"),
                    DispatchConsumersAsync = true
                },
                _ => throw new InvalidOperationException()
            };

            return factory.CreateConnection();
        }
    }
}
