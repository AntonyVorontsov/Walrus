using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Walrus.Producer;

internal sealed class ProducerFactory : IProducerFactory
{
    private readonly ProducerConfig _producerConfig;
    private readonly ILogger _logger;

    public ProducerFactory(ProducerConfig producerConfig, ILogger<ProducerFactory> logger)
    {
        _producerConfig = producerConfig;
        _logger = logger;
    }

    public IProducer<byte[]?, byte[]?> Create()
    {
        return new ProducerBuilder<byte[]?, byte[]?>(_producerConfig)
            // TODO: Log handlers.
            .Build();
    }
}