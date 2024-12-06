using Microsoft.AspNetCore.SignalR;
using MSMQ.Common.Messages;
using MSMQ.Common.Serializers;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            await Publish(message, cancellationToken);
        }

        public async Task Publish(CommonMessage message, CancellationToken cancellationToken)
        {
            try
            {
                string qName = _qGenerator.GetQueue(message);
                byte[] serializedMessage = MessageSerializer.Serialize(message);
                IBasicProperties properties = CreateMessageProperties(message);

                _logger.LogInformation("Started publishing message #{MessageId} to queue '{QueueName}'", message.Id, qName);
                _channel.BasicPublish(exchange: "", routingKey: qName, basicProperties: null, body: serializedMessage);

                if(!_channel.WaitForConfirms(TimeSpan.FromSeconds(15)))//apenas por demonstração
                    _logger.LogInformation("Unable to confirm whether the message was published or not #{MessageId} to queue '{QueueName}'", message.Id, qName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while publishing the message, reason: {ErrorReason}", e.Message);
            }
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
