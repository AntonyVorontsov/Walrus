using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Walrus.Configuration;
using Walrus.Producer.Configuration;

namespace Walrus.Producer;

public sealed class KafkaProducer<TKey, TBody> : IKafkaProducer<TKey, TBody>
{
    private readonly IBaseProducer _baseProducer;
    private readonly IEnumerable<IKafkaHeaderEnricher> _enrichers;
    private readonly KafkaProducerConfiguration<TKey, TBody> _configuration;

    public KafkaProducer(
        IBaseProducer baseProducer,
        IEnumerable<IKafkaHeaderEnricher> enrichers,
        IOptions<KafkaProducerConfiguration<TKey, TBody>> producerConfiguration)
    {
        _baseProducer = baseProducer;
        _enrichers = enrichers;
        _configuration = producerConfiguration.Value;
    }

    public async ValueTask Produce(TKey key, TBody body, CancellationToken cancellationToken)
    {
        var message = new Message<byte[]?, byte[]?>
        {
            Key = _configuration.KeySerializer.Serialize(key),
            Value = _configuration.BodySerializer.Serialize(body),
            Headers = ExtractHeaders()
        };
        await _baseProducer.Produce(_configuration.Topic, message, cancellationToken);
    }

    public async ValueTask Produce(IReadOnlyCollection<KafkaMessage<TKey, TBody>> kafkaMessages,
        CancellationToken cancellationToken)
    {
        var headers = ExtractHeaders();
        var messages = kafkaMessages.Select(
                message => new Message<byte[]?, byte[]?>
                {
                    Key = _configuration.KeySerializer.Serialize<TKey>(message.Key),
                    Value = _configuration.BodySerializer.Serialize<TBody>(message.Body),
                    Headers = headers
                })
            .ToArray();
        await _baseProducer.Produce(_configuration.Topic, messages, cancellationToken);
    }

    private Headers ExtractHeaders()
    {
        return KafkaHeadersBuilder.Create(
                _enrichers.Select(x => x.GetHeader())
                    .Where(header => header is not null)
                    .ToArray()!)
            .Build();
    }
}