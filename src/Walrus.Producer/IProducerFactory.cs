using Confluent.Kafka;

namespace Walrus.Producer;

public interface IProducerFactory
{
    public IProducer<byte[]?, byte[]?> Create();
}