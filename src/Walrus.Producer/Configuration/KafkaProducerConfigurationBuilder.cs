using Microsoft.Extensions.DependencyInjection;
using Walrus.Producer.Serializers;

namespace Walrus.Producer.Configuration;

public sealed class KafkaProducerConfigurationBuilder<TKey, TBody>
{
    private readonly IServiceCollection _services;

    public KafkaProducerConfigurationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public KafkaProducerConfigurationBuilder<TKey, TBody> SetKeySerializer<TSerializer>(TSerializer serializer)
        where TSerializer : IKafkaMessageSerializer
    {
        _services.Configure<KafkaProducerConfiguration<TKey, TBody>>(
            config =>
            {
                config.KeySerializer = serializer;
            });

        return this;
    }

    public KafkaProducerConfigurationBuilder<TKey, TBody> SetBodySerializer<TSerializer>(TSerializer serializer)
        where TSerializer : IKafkaMessageSerializer
    {
        _services.Configure<KafkaProducerConfiguration<TKey, TBody>>(
            config =>
            {
                config.BodySerializer = serializer;
            });

        return this;
    }
}