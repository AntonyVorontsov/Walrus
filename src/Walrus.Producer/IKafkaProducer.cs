using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Walrus.Configuration;

namespace Walrus.Producer;

public interface IKafkaProducer<TKey, TBody>
{
    ValueTask Produce(
        TKey key,
        TBody body,
        CancellationToken cancellationToken);

    ValueTask Produce(
        IReadOnlyCollection<KafkaMessage<TKey, TBody>> kafkaMessages,
        CancellationToken cancellationToken);
}