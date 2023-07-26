using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Kafka;
using CloudNative.CloudEvents.NewtonsoftJson;
using Confluent.Kafka;
using integration.services.kafka.shared.converters;
using integration.services.kafka.shared.interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace integration.services.kafka.shared.producer
{
    public class Producer : IProducer
    {
        private ProducerConfig _producerConfig;
        private ProducerBuilder<string?, byte[]> _producerBuilder;
        private IProducer<string?, byte[]> _producer;

        public Producer(ITopicConnectionOptions producerTopicConnectionOptions)
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = producerTopicConnectionOptions.Host,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = producerTopicConnectionOptions.Username,
                SaslPassword = producerTopicConnectionOptions.Key,
                MessageMaxBytes = producerTopicConnectionOptions.MessageMaxBytes
            };
        }

        // Used only for testing 
        public Producer(ITopicConnectionOptions producerTopicConnectionOptions, ProducerBuilder<string?, byte[]> producerBuilder, IProducer<string?, byte[]> producer)
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = producerTopicConnectionOptions.Host,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = producerTopicConnectionOptions.Username,
                SaslPassword = producerTopicConnectionOptions.Key,
                MessageMaxBytes = producerTopicConnectionOptions.MessageMaxBytes
            };

            _producerBuilder = producerBuilder;
            _producer = producer;
        }

        public async virtual Task<DeliveryResult<string?, byte[]>> PublishAsync(string topic, object message)
        {
            var eventMessageHandler = new EventMessageConverter();
            var kafkaMessage = eventMessageHandler.CreateMessage<CloudEvent>(message).ToKafkaMessage(ContentMode.Binary, new JsonEventFormatter());
            _producerBuilder ??= new ProducerBuilder<string?, byte[]>(_producerConfig);

            using (_producer ??= _producerBuilder.Build())
            {
                try
                {
                    DeliveryResult<string?, byte[]> deliveryDetails = await _producer.ProduceAsync(topic, kafkaMessage);
                    // Setting to null since the response size is too high
                    deliveryDetails.Headers = null;
                    deliveryDetails.Value = null;
                    return deliveryDetails;
                }
                catch (ProduceException<string, string> e)
                {
                    throw new Exception($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
            }
        }
    }

}
