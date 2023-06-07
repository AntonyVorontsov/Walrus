using System.Collections.Generic;
using Confluent.Kafka;

namespace Walrus.Producer;

internal sealed class KafkaHeadersBuilder
{
    private readonly Headers _headers;

    private KafkaHeadersBuilder()
    {
        _headers = new Headers();
    }

    internal static KafkaHeadersBuilder Create(IEnumerable<Header> headers)
    {
        var builder = new KafkaHeadersBuilder();
        foreach (var header in headers)
        {
            builder.Add(header);
        }

        return builder;
    }

    private void Add(Header header)
    {
        _headers.Add(header);
    }

    internal Headers Build()
    {
        return _headers;
    }
}