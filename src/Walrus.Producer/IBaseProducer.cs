using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Walrus.Producer;

public interface IBaseProducer : IDisposable
{
    ValueTask Produce(
        string topic,
        Message<byte[]?, byte[]?> message,
        CancellationToken cancellationToken);

    ValueTask Produce(
        string topic,
        IReadOnlyCollection<Message<byte[]?, byte[]?>> messages,
        CancellationToken cancellationToken);
}