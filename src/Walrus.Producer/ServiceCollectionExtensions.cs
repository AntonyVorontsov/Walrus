using System;
using System.Linq;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Walrus.Configuration;
using Walrus.Producer.Configuration;
using Walrus.Producer.Serializers.Extensions;

namespace Walrus.Producer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducer<TKey, TBody>(
        this IServiceCollection services,
        string topic,
        Action<KafkaProducerConfigurationBuilder<TKey, TBody>>? builder = null)
    {
        if (string.IsNullOrEmpty(topic))
        {
            throw new ArgumentException("You have to provide a topic name when registering a kafka producer",
                nameof(topic));
        }

        if (AlreadyRegistered<KafkaProducer<TKey, TBody>>(services))
        {
            return services;
        }

        // TODO: consider moving to IOptions<ProducerConfig>.
        services.SetGlobalKafkaProducerConfiguration();

        services.TryAddSingleton<IProducerFactory, ProducerFactory>();
        services.TryAddSingleton<IBaseProducer, BaseProducer>();

        services.Configure<KafkaProducerConfiguration<TKey, TBody>>(
            configuration =>
            {
                configuration.Topic = topic;
                configuration.KeySerializer = SerializerDetectionExtensions.GetDefaultSerializerOf<TKey>();
                configuration.BodySerializer = SerializerDetectionExtensions.GetDefaultSerializerOf<TBody>();
            });
        services.TryAddSingleton<IKafkaProducer<TKey, TBody>, KafkaProducer<TKey, TBody>>();

        var builderInstance = new KafkaProducerConfigurationBuilder<TKey, TBody>(services);
        builder?.Invoke(builderInstance);

        return services;
    }

    public static IServiceCollection AddKafkaProducerHeaderEnricher<TEnricher>(this IServiceCollection services)
        where TEnricher : class, IKafkaHeaderEnricher
    {
        services.TryAddTransient<IKafkaHeaderEnricher, TEnricher>();

        return services;
    }

    // TODO: consider moving to IOptions<ProducerConfig>.
    public static IServiceCollection SetGlobalKafkaProducerConfiguration(
        this IServiceCollection services,
        Action<ProducerConfig>? configuration = null)
    {
        var producerConfig = new ProducerConfig();
        configuration?.Invoke(producerConfig);

        services.TryAddSingleton(
            provider =>
            {
                if (!string.IsNullOrEmpty(producerConfig.BootstrapServers)) return producerConfig;

                var brokers = provider.GetRequiredService<ConnectionConfiguration>().BootstrapServers;
                if (string.IsNullOrEmpty(brokers))
                {
                    throw new InvalidOperationException(
                        $"Brokers are not configured. Use {nameof(Walrus.Configuration.ServiceCollectionExtensions.AddKafka)} beforehand");
                }

                producerConfig.BootstrapServers = brokers;
                return producerConfig;
            });

        return services;
    }

    private static bool AlreadyRegistered<TImplementation>(IServiceCollection services)
    {
        return services.Any(x => x.ImplementationType == typeof(TImplementation));
    }
}