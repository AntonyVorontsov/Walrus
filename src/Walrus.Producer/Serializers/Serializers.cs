namespace Walrus.Producer.Serializers;

public static class Serializers
{
    public static class Default
    {
        public static readonly IJsonKafkaMessageSerializer JsonKafkaMessage = new JsonKafkaMessageSerializer();
        public static readonly IProtoKafkaMessageSerializer ProtoKafkaMessage = new ProtobufKafkaMessageSerializer();
        public static readonly IXmlKafkaMessageSerializer XmlKafkaMessage = new XmlKafkaMessageSerializer();
        public static readonly IKafkaMessageSerializer Byte = new ByteKafkaMessageSerializer();
    }
}