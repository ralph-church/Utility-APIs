using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using System;
using System.Threading.Tasks;

namespace repair.service.service
{
    public class RepairRequestStageService : BaseService<RepairRequestStageModel, RepairRequestStage>,IRepairRequestStageService
    {
        private readonly ILogger<RepairRequestStageService> _logger;           
        public RepairRequestStageService(ILogger<RepairRequestStageService> logger,                                   
                                   IDataRepository<RepairRequestStage> repository,
                                   IMapper mapper) : base(repository, mapper)
        {
            _logger = logger;
        }

        public override async Task<RepairRequestStageModel> InsertOneAsync(RepairRequestStageModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke InsertOneAsync {JsonConvert.SerializeObject(model)}");
                var eventModel = await base.InsertOneAsync(model);                
                return eventModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Event services with error{ex.Message}");                
                throw;
            }
        }


        public override async Task<RepairRequestStageModel> ReplaceOneAsync(RepairRequestStageModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke InsertOneAsync {JsonConvert.SerializeObject(model)}");
                var eventModel = await base.ReplaceOneAsync(model);                                
                return eventModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"event services with error{ex.Message}");                
                throw;
            }
        }
    }
}
