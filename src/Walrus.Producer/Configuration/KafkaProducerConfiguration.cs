using Walrus.Producer.Serializers;
// ReSharper disable UnusedTypeParameter

namespace Walrus.Producer.Configuration;

public sealed class KafkaProducerConfiguration<TKey, TBody>
{
    public string Topic { get; set; }
    public IKafkaMessageSerializer KeySerializer { get; set; }
    public IKafkaMessageSerializer BodySerializer { get; set; }
}