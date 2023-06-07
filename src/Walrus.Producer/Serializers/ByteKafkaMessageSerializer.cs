using Walrus.Producer.Exceptions;

namespace Walrus.Producer.Serializers;

internal sealed class ByteKafkaMessageSerializer : IKafkaMessageSerializer
{
    public byte[]? Serialize<T>(T value)
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new KafkaProducerMessageSerializationException(
                typeof(T),
                "Byte serializer can only handle raw byte[] messages");
        }

        return value as byte[];
    }
}