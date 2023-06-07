namespace Walrus.Producer.Serializers;

public interface IKafkaMessageSerializer
{
    public byte[]? Serialize<T>(T value);
}