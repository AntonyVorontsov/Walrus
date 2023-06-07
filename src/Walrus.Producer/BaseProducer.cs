using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Walrus.Producer.Exceptions;

namespace Walrus.Producer;

internal sealed class BaseProducer : IBaseProducer
{
    private readonly IProducer<byte[]?, byte[]?> _producer;

    public BaseProducer(IProducerFactory producerFactory)
    {
        _producer = producerFactory.Create();
    }
    
    public void Dispose()
    {
        _producer?.Dispose();
    }


    public async ValueTask Produce(
        string topic,
        Message<byte[]?, byte[]?> message,
        CancellationToken cancellationToken)
    {
        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public ValueTask Produce(
        string topic,
        IReadOnlyCollection<Message<byte[]?, byte[]?>> messages,
        CancellationToken cancellationToken)
    {
        if (messages.Count == 0)
        {
            return ValueTask.CompletedTask;
        }

        var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var registration = cancellationToken.Register(() => completionSource.TrySetCanceled(cancellationToken));

        var messagesToDeliver = messages.Count;
        var deliveredMessages = 0;

        foreach (var message in messages)
        {
            _producer.Produce(
                topic,
                message,
                deliveryReport =>
                {
                    if (deliveryReport.Error is not null &&
                        deliveryReport.Error.Code is not ErrorCode.NoError)
                    {
                        var exception = new KafkaMessageDeliveryException(deliveryReport.Error.Code, deliveryReport.Error.Reason);
                        completionSource.TrySetException(exception);
                    }
                    else
                    {
                        Interlocked.Increment(ref deliveredMessages);
                        if (messagesToDeliver == deliveredMessages)
                        {
                            completionSource.TrySetResult();
                        }
                    }
                });
        }

        return new ValueTask(completionSource.Task);
    }
}