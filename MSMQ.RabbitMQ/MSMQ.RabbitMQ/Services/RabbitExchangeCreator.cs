using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitExchangeCreator
    {
        void Create(string name);
        void Create(string name, string type);
    }

    public class RabbitExchangeCreator(IConnection _connection) : IRabbitExchangeCreator
    {
        private readonly IModel _channel = _connection.CreateModel();

        public void Create(string name) => _channel.ExchangeDeclare(name, ExchangeType.Direct, durable: true, autoDelete: false, null);

        public void Create(string name, string type) => _channel.ExchangeDeclare(name, type, durable: true, autoDelete: false, null);
    }
}
