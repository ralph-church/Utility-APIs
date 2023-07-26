using Confluent.Kafka;
using System.Threading.Tasks;

namespace integration.services.kafka.shared.interfaces
{
    public interface IProducer
    {
        Task<DeliveryResult<string?, byte[]>> PublishAsync(string topic, object message);
    }
}
