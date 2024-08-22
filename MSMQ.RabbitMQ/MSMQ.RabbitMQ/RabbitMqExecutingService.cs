using Microsoft.Extensions.Configuration;
using MSMQ.Common;
using MSMQ.Common.Serializers;
using MSMQ.RabbitMQ.Factories;
using MSMQ.RabbitMQ.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MSMQ.RabbitMQ
{
    public interface IRabbitMqExecutingService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }

    public class RabbitMqExecutingService(ILogger<Worker> _logger, IConfiguration _configuration, IRabbitMessagesQueueCreator _messagesQueueCreator,
            IRabbitQueuesGenerator _queuesGenerator, IRabbitConsumerFactory _consumersFactory, IConnection _connection) : IRabbitMqExecutingService
    {
        private readonly IModel _channel = _connection.CreateModel();

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

                _logger.LogInformation("RabbitMq Services are being iniciated...");

                var exchange = _configuration.GetValue<string>("RabbitMq:Exchange");
                var queues = _queuesGenerator.GetQueues();

                _messagesQueueCreator.CreateQueues(exchange, queues.ToArray());

                foreach (var q in queues)
                    _logger.LogInformation("RabbitMq services have subscribed to the following queue: '{QueueName}'", q);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += async (a, @event) =>
                {
                    try
                    {
                        var content = @event.Body;
                        var message = MessageSerializer.Deserialize(content.Span);
                        var handlers = _consumersFactory.Create(message);

                        foreach (var handler in handlers)
                        {
                            await handler.Consume(message);
                        }

                        _channel.BasicAck(@event.DeliveryTag, false);

                        _logger.LogInformation("Finished consuming messaged '#{MessageId}' completely, it'll be removed from queue. It took {MsTime}", message.Id, message.Time.GetAmountTimeTookMS(DateTimeOffset.UtcNow));
                    }
                    catch (Exception)
                    {
                        _channel.BasicNack(@event.DeliveryTag, false, true);
                    }
                };

                foreach (var q in queues)
                    _channel.BasicConsume(queue: q, autoAck: false, consumer: consumer);

                _logger.LogInformation("RabbitMq Services are waiting for messages...");

            }
            catch (OperationCanceledException e)
            {
                _logger.LogInformation("RabbitMq Services are being shutdown...");
            }
        }

    }
}
