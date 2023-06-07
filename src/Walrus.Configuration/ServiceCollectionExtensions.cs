using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Walrus.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, string brokers)
    {
        if (string.IsNullOrEmpty(brokers)) throw new ArgumentException("Brokers should not be null or empty", nameof(brokers));

        services.AddOptions();
        services.TryAddSingleton(new ConnectionConfiguration(brokers));
        return services;
    }

    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionConfiguration = configuration.Get<ConnectionConfiguration>();
        if (string.IsNullOrEmpty(connectionConfiguration?.BootstrapServers))
        {
            throw new ArgumentException("Brokers should not be null or empty", nameof(configuration));
        }

        services.AddOptions();
        services.TryAddSingleton(connectionConfiguration);
        return services;
    }
}