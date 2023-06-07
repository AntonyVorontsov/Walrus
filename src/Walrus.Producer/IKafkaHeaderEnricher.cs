using Confluent.Kafka;

namespace Walrus.Producer;

public interface IKafkaHeaderEnricher
{
    Header? GetHeader();
}