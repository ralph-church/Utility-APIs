using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.abstracts;
using repair.service.shared.paging;
using System;
using System.Threading.Tasks;

namespace repair.service.service
{
    public class RepairInvoiceStageService : BaseService<RepairInvoiceStageModel, RepairInvoiceStage>, IRepairInvoiceStageService
    {
        private readonly ILogger<IRepairInvoiceStageService> _logger;
        private readonly IHttpService _httpService;

        public RepairInvoiceStageService(ILogger<IRepairInvoiceStageService> logger,
                                   IDataRepository<RepairInvoiceStage> repository,
                                   IMapper mapper, IHttpService httpService) : base(repository, mapper)
        {
            _logger = logger;
            _httpService = httpService;
        }


        public override async Task<PagedResponseList<RepairInvoiceStageModel>> FindAsync(string groupKey, int maxRetryCount,
                                                                               QueryStringParams queryStringParams = null)
        {
            return await base.FindAsync(groupKey, maxRetryCount, queryStringParams);
        }


        public override async Task<RepairInvoiceStageModel> InsertOneAsync(RepairInvoiceStageModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke InsertOneAsync {JsonConvert.SerializeObject(model)}");
                var repairInvoiceModel = await base.InsertOneAsync(model);
                return repairInvoiceModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Repair Invoice with error{ex.Message}");
                throw;
            }
        }
        public override async Task<RepairInvoiceStageModel> ReplaceOneAsync(RepairInvoiceStageModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke ReplaceOneAsync {JsonConvert.SerializeObject(model)}");
                var repairInvoiceModel = await base.ReplaceOneAsync(model);
                return repairInvoiceModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Repair Invoice with error{ex.Message}");
                throw;
            }
        }

        public async Task<RepairInvoiceModel> FindOneAsync(int orderId, string AccountId)
        {
            return await base.FindOneAsync(model => model.OrderId == orderId && model.AccountId == AccountId);
        }

    }
}
