using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitQueueCreator
    {
        void Create(string name);
        void Create(string name, string exchange);
    }

    public class RabbitQueueCreator(IConnection _connection) : IRabbitQueueCreator
    {
        public void Create(string name)
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(name, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }


        public void Create(string name, string exchange)
        {
            using var channel = _connection.CreateModel();

            //mudar routing key para o name da queue
            channel.QueueDeclare(name, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: name, exchange: exchange, routingKey: string.Empty, arguments: null);
        }
    }
}
