using System;
using Confluent.Kafka;
using Google.Protobuf;
using Walrus.Producer.Exceptions;

namespace Walrus.Producer.Serializers;

internal sealed class ProtobufKafkaMessageSerializer : IProtoKafkaMessageSerializer
{
    public byte[]? Serialize<T>(T value)
    {
        if (value is not IMessage data)
        {
            throw new KafkaProducerMessageSerializationException(
                typeof(T),
                $"For proper proto serialization an object has to implement {nameof(IMessage)}");
        }

        if (typeof(T) == typeof(Null) || typeof(T) == typeof(Ignore))
        {
            return null;
        }

        try
        {
            return data.ToByteArray();
        }
        catch (Exception exception)
        {
            throw new KafkaProducerMessageSerializationException(typeof(T), exception);
        }
    }
}