using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Walrus.Producer.Exceptions;

namespace Walrus.Producer.Serializers;

internal sealed class JsonKafkaMessageSerializer : IJsonKafkaMessageSerializer
{
    private readonly JsonSerializerSettings _serializationSettings;

    public JsonKafkaMessageSerializer()
    {
        _serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };
    }

    public byte[]? Serialize<T>(T value)
    {
        if (typeof(T) == typeof(Null) || typeof(T) == typeof(Ignore))
        {
            return null;
        }

        try
        {
            var jsonString = JsonConvert.SerializeObject(value, _serializationSettings);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        catch (Exception exception)
        {
            throw new KafkaProducerMessageSerializationException(typeof(T), exception);
        }
    }
}