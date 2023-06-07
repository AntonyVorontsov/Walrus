using System;
using System.Collections.Concurrent;
using System.Xml.Serialization;
using Google.Protobuf;
using static Walrus.Producer.Serializers.Serializers;

namespace Walrus.Producer.Serializers.Extensions;

internal static class SerializerDetectionExtensions
{
    internal static IKafkaMessageSerializer GetDefaultSerializerOf<T>()
    {
        var messageType = typeof(T);
        return GetDefaultSerializerOfType(messageType);
    }

    private static IKafkaMessageSerializer GetDefaultSerializerOfType(Type type)
    {
        if (type == typeof(byte[]))
        {
            return Default.Byte;
        }

        if (typeof(IMessage).IsAssignableFrom(type))
        {
            return Default.ProtoKafkaMessage;
        }

        if (Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute)) != null)
        {
            return Default.XmlKafkaMessage;
        }

        return Default.JsonKafkaMessage;
    }
}