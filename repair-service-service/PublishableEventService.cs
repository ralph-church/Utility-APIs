using AutoMapper;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.service.model
{
    public class PublishableEventService : BaseService<PublishableEventModel, PublishEvents>, IPublishableEventService
    {
        private readonly IDataRepository<PublishEvents> _servicePublishableEventRepository;
        public PublishableEventService(IDataRepository<PublishEvents> repository, IMapper mapper) : base(repository, mapper)
        {
            _servicePublishableEventRepository = repository;
        }
     
        public async Task<PublishEvents> FindAsync(string eventType)
        {
            var events = await _servicePublishableEventRepository.FindOneAsync(model => model.EventType == eventType && model.IsActive );
            return events;
        }
    }
}
