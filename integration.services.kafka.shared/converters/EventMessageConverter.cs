using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Kafka;
using Confluent.Kafka;
using integration.services.kafka.shared.interfaces;
using System;
using System.Net.Mime;


namespace integration.services.kafka.shared.converters
{
    public class EventMessageConverter : IEventMessageConverter
    {
        public Message<string?, byte[]> ToKafkaMessages(CloudEvent cloudEvent, ContentMode contentMode, CloudEventFormatter formatter)
        {
            if (cloudEvent != null)
            {
                return cloudEvent.ToKafkaMessage(contentMode, formatter);
            }
            return new Message<string?, byte[]>();
        }

        public CloudEvent ToCloudEvent(Message<string?, byte[]> message, CloudEventFormatter formatter, params CloudEventAttribute[]? extensionAttributes)
        {
            if (message != null)
            {
                return message.ToCloudEvent(formatter, extensionAttributes);
            }
            return new CloudEvent();
        }

        public virtual T CreateMessage<T>(object message)
        {
            if (typeof(T) == typeof(CloudEvent))
            {
                var cloudEvent = new CloudEvent
                {
                    Type = (string)message.GetType().GetProperty("EventType")?.GetValue(message),
                    Source = (Uri)message.GetType().GetProperty("EventSource")?.GetValue(message),
                    Subject = (string)message.GetType().GetProperty("EventSubject")?.GetValue(message),
                    Id = (string)message.GetType().GetProperty("EventId")?.GetValue(message),
                    Time = new DateTimeOffset(DateTime.Now),
                    DataContentType = MediaTypeNames.Application.Json,
                    Data = (string)message.GetType().GetProperty("Data")?.GetValue(message),
                };
                return (T)Convert.ChangeType(cloudEvent, typeof(T));
            }
            return (T)Convert.ChangeType(new object(), typeof(T));
        }
    }
}
