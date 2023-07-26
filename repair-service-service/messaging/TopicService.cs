using Confluent.Kafka;
using integration.services.kafka.shared.producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace repair.service.service.messaging
{
    public class TopicService : IOutboundService
    {
        private readonly List<ProducerConnectionOptions> _producerConnectionOptions;
        private readonly IServiceScope _scope;
        private readonly ILogger<IOutboundService> _logger;
        private readonly string _producerCryptoKey;
        public TopicService(ILogger<IOutboundService> logger, IOptions<List<ProducerConnectionOptions>> producerConnectionOptions, IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateScope();
            _logger = logger;
            _producerConnectionOptions = producerConnectionOptions.Value;
            var config =_scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _producerCryptoKey = config?.GetSection("ProducerCryptoKey")?.Value;
        }

        public async ValueTask<object> PublishAsync(EventPublishModel message)
        {
            IPublishableEventService _publishableEvent = _scope.ServiceProvider.GetRequiredService<IPublishableEventService>();
            var pcOptions = await _publishableEvent.FindAsync(message.EventType);
            try
            {
                if (pcOptions != null)
                {
                    ProducerConnectionOptions producerConnectionOptions = new ProducerConnectionOptions()
                    {
                        EntityName = pcOptions.EntityName,
                        Host = pcOptions.Host,
                        Key = await CryptoHelper.DecryptAsync(pcOptions.Key, _producerCryptoKey),
                        Username = await CryptoHelper.DecryptAsync(pcOptions.UserName, _producerCryptoKey),
                        InboundSubscription = pcOptions.InboundSubscription,
                        MessageMaxBytes = pcOptions.MessageMaxBytes
                    };
                    var _producer = new Producer(producerConnectionOptions);
                    _logger.LogInformation($"Publishing the message to kafa topic - {producerConnectionOptions.EntityName} ");
                    return await _producer.PublishAsync(producerConnectionOptions.EntityName, CreateOutgoingMessage(message));
                }
                else
                {
                    throw new Exception($"Cannot derive the producer connection option for the topic type - {message.EventType} ");
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        private ProducerConnectionOptions GetProducerConnectionOption(string topicType)
        {
            if (string.IsNullOrWhiteSpace(topicType))
            {
                _logger.LogError("Topic Type is Empty in the payload");
                return null;
            }

            return _producerConnectionOptions.FirstOrDefault(p => p.TopicType.Trim() == topicType.Trim());
        }

        private IOutgoingMessage CreateOutgoingMessage(EventPublishModel message)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Data = message.Data,
                EventId = message.EventId,
                EventSource = message.EventSource,
                EventSubject = message.EventSubject,
                EventType = message.EventType
            };
            _logger.LogInformation($"Input data for Create Repair Request / Create Note:{ JsonConvert.SerializeObject(outgoingMessage) }");
            return outgoingMessage;
        }

    }
}
