using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Confluent.Kafka;
using integration.services.kafka.shared.converters;
using Newtonsoft.Json;
using CloudNative.CloudEvents.Kafka;
using CloudNative.CloudEvents.NewtonsoftJson;

namespace integration.service.kafka.test.mocks.data
{
    public class ProducerMockData
    {
        public ProducerMockData() { }        
        public async Task<DeliveryResult<string?, byte[]>> ProduceDeliveryDetails()
        {            
            var kafkaMessages = GetCloudEventMessage().ToKafkaMessage(ContentMode.Binary, new JsonEventFormatter());
            using (var producerBuild = new ProducerBuilder<string?, byte[]>(new ProducerConfig()).Build())
            {
                try
                {                    
                    var deliveryDetails = new DeliveryResult<string, byte[]>
                    {             
                        Status = PersistenceStatus.Persisted,
                        Message = kafkaMessages
                    };                   
                    return deliveryDetails;
                }
                catch (ProduceException<string, string> e)
                {
                    throw new Exception($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
            }
        }

        private CloudEvent GetCloudEventMessage()
        {
            return new CloudEvent
            {
                Type = "com.github.pull.create",
                Source = new Uri("https://github.com/cloudevents/spec/pull"),
                Subject = "123",
                Id = "A234-1234-1234",
                Time = new DateTimeOffset(2018, 4, 5, 17, 31, 0, TimeSpan.Zero),
                DataContentType = MediaTypeNames.Text.Xml,
                Data = "My data"
            };
        }        
    }
}
