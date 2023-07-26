using CloudNative.CloudEvents;
using Confluent.Kafka;

namespace integration.services.kafka.shared.interfaces
{
    public interface IEventMessageConverter
    {
        public T CreateMessage<T>(object message);
        public Message<string?, byte[]> ToKafkaMessages(CloudEvent cloudEvent, ContentMode contentMode, CloudEventFormatter formatter);
        public CloudEvent ToCloudEvent(Message<string?, byte[]> message, CloudEventFormatter formatter, params CloudEventAttribute[]? extensionAttributes);
    }
}
