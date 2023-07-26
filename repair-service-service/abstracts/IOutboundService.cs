using repair.service.service.model;
using System.Threading;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IOutboundService
    {
        ValueTask<object> PublishAsync(EventPublishModel message);       
    }
}
