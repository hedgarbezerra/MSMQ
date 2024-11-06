using Azure.Messaging.ServiceBus;
using MSMQ.Bus.Factories;
using MSMQ.Bus.Services;
using System;
using System.Threading.Tasks;

public interface IQueueSubscriber
{
    Task Subscribe(CancellationToken cancellationToken);
    Task Subscribe(IEnumerable<string> queues, CancellationToken cancellationToken);
    Task Unsubscribe(CancellationToken cancellationToken);
    Task Unsubscribe(IEnumerable<string> queueNames, CancellationToken cancellationToken);
}

public class QueueSubscriber : IQueueSubscriber
{
    private readonly ILogger<QueueSubscriber> _logger;
    private readonly IMessageHandlersFactory _handlersFactory;
    private readonly Dictionary<string, ServiceBusProcessor> _processors;

    public QueueSubscriber(ILogger<QueueSubscriber> logger, IMessageHandlersFactory handlersFactory, ServiceBusClient client, IServiceBusQueueGenerator queueGenerator)
    {
        _logger = logger;
        _handlersFactory = handlersFactory;
        _processors = queueGenerator.GetQueues()
            .ToDictionary(q => q, q => client.CreateProcessor(q));
    }

    public async Task Subscribe(CancellationToken cancellationToken)
    {
        foreach (var (queueName, processor) in _processors)
        {
            var handlers = _handlersFactory.Create(queueName);
            Func<ProcessMessageEventArgs, Task> handlerFunc = async (@event) =>
            {
                foreach (var handler in handlers)
                {
                    await handler.Handle(@event, cancellationToken);
                }
            };
            await Subscribe(queueName, handlerFunc, cancellationToken);
        }
    }

    public async Task Subscribe(IEnumerable<string> queues, CancellationToken cancellationToken = default)
    {
        foreach (var queueName in queues)
        {
            var handlers = _handlersFactory.Create(queueName);
            Func<ProcessMessageEventArgs, Task> handlerFunc = async (@event) =>
            {
                foreach (var handler in handlers)
                {
                    await handler.Handle(@event, cancellationToken);
                }
            };
            await Subscribe(queueName, handlerFunc, cancellationToken);
        }
    }

    private async Task Subscribe(string queueName, Func<ProcessMessageEventArgs, Task> handlerFunc, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Subscribing to ServiceBus' queue '{QueueName}'", queueName);

        if (!_processors.TryGetValue(queueName, out var processor))
            throw new InvalidOperationException($"Message processor not implemented for queue '{queueName}'");

        processor.ProcessMessageAsync += handlerFunc;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync(cancellationToken);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError("Error occured while processing message from '{QueueName}' queue, '{ErrorReason}'.", args.Identifier, args.Exception.Message);
        return Task.CompletedTask;
    }

    public async Task Unsubscribe(CancellationToken cancellationToken)
    {
        foreach (var (queueName, processor) in _processors)
            await Unsubscribe(queueName, cancellationToken);
    }

    public async Task Unsubscribe(IEnumerable<string> queueNames, CancellationToken cancellationToken)
    {
        foreach (var queueName in queueNames)
            await Unsubscribe(queueName, cancellationToken);
    }

    private async Task Unsubscribe(string queueName, CancellationToken cancellationToken = default)
    {
        if (!_processors.TryGetValue(queueName, out var processor))
            throw new InvalidOperationException($"Message processor not implemented for queue '{queueName}'");

        await processor.StopProcessingAsync(cancellationToken);
        await processor.DisposeAsync();
        _logger.LogInformation("Unsubscribing to ServiceBus' queue '{QueueName}'", queueName);
    }
}
