using System;
using Confluent.Kafka;

namespace Walrus.Producer.Exceptions;

public sealed class KafkaMessageDeliveryException : Exception
{
    public KafkaMessageDeliveryException(ErrorCode errorCode, string reason)
        : base($"Walrus.Producer. Kafka message delivery occured. Error code: {errorCode}. Reason {reason}")
    {
    }
}