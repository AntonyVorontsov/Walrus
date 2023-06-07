using System;

namespace Walrus.Producer.Exceptions;

public sealed class KafkaProducerMessageSerializationException : Exception
{
    public KafkaProducerMessageSerializationException(Type type, string message)
        : base($"Walrus.Producer. Could not serialize an instance of type {type}. Reason: {message}")
    {
    }

    public KafkaProducerMessageSerializationException(Type type, Exception exception)
        : base($"Walrus.Producer. Could not serialize an instance of type {type}. Reason in inner exception", exception)
    {
    }
}