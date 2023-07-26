using repair.service.data.entities;
using repair.service.service.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IPublishableEventService:IBaseService<PublishableEventModel, PublishEvents>
    {
        Task<PublishEvents> FindAsync(string eventType);
    }
}
