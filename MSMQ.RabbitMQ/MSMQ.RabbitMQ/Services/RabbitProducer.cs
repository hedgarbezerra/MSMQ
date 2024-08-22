using MSMQ.Common.Messages;
using MSMQ.Common.Serializers;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ.Services
{
    public interface IRabbitProducer
    {
        public Task Publish<TPayload>(TPayload payload, CancellationToken cancellationToken);
        public Task Publish(CommonMessage message, CancellationToken cancellationToken);
    }

    public class RabbitProducer(ILogger<RabbitProducer> _logger, IConnection _connection, IConfiguration _configuration, IRabbitQueuesGenerator _qGenerator) : IRabbitProducer
    {
        private readonly string _exchange = _configuration.GetValue<string>("RabbitMq:Exchange");
        private readonly IModel _channel = _connection.CreateModel();

        public async Task Publish<TPayload>(TPayload payload, CancellationToken cancellationToken)
        {
            CommonMessage message = CommonMessage.Create(payload);
            string qName = _qGenerator.GetQueue(message);
            byte[] serializedMessage = MessageSerializer.Serialize(message);
            IBasicProperties properties = CreateMessageProperties(message);

            _logger.LogInformation("Started publishing message #{MessageId} to queue '{QueueName}'", message.Id, _exchange);
             _channel.BasicPublish(exchange: "", routingKey: qName, basicProperties: null, body: serializedMessage);
        }

        public async Task Publish(CommonMessage message, CancellationToken cancellationToken)
        {
            string qName = _qGenerator.GetQueue(message);
            byte[] serializedMessage = MessageSerializer.Serialize(message);
            IBasicProperties properties = CreateMessageProperties(message);

            _logger.LogInformation("Started publishing message #{MessageId} to queue '{QueueName}'", message.Id, _exchange);
            _channel.BasicPublish(exchange: "", routingKey: qName, basicProperties: null, body: serializedMessage);
        }

        private IBasicProperties CreateMessageProperties(CommonMessage message)
        {
            var props = _channel.CreateBasicProperties();

            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
            props.Expiration = "100000";
            props.CorrelationId = message.Id.ToString();

            return props;
        }    
    }
}
