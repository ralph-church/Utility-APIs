using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.abstracts;
using repair.service.shared.model;
using repair.service.shared.constants;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace repair.service.service
{
    public class RepairInvoiceService : BaseService<RepairInvoiceModel, RepairInvoice>, IRepairInvoiceService
    {
        private readonly ILogger<IRepairInvoiceService> _logger;
        private readonly IHttpService _httpService;
        private readonly ITtcServicesConfig _ttcServiceConfig;


        public RepairInvoiceService(ILogger<IRepairInvoiceService> logger,
                                   IDataRepository<RepairInvoice> repository,
                                   IMapper mapper, IHttpService httpService, ITtcServicesConfig ttcServiceConfig) : base(repository, mapper)
        {
            _logger = logger;
            _ttcServiceConfig = ttcServiceConfig;
            _httpService = httpService;
        }
        public async Task<RepairInvoiceModel> InsertOneAsync(RepairInvoiceModel model, JwtInfo jwtInfo)
        {
            try
            {
                _logger.LogInformation($"Invoke InsertOneAsync {JsonConvert.SerializeObject(model)}");
                var response = await SaveOnPremAsync(model, jwtInfo);
                if (response != null && response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"SaveOnPremAsync - Success");

                    //Deserialize the result content from SaveOnPrem to RepairInvoiceModel 
                    RepairInvoiceModel repairInvoiceModel = JsonConvert.DeserializeObject<RepairInvoiceModel>(await response.Content?.ReadAsStringAsync());

                    _logger.LogInformation($"SaveOnPremAsync - Success {JsonConvert.SerializeObject(repairInvoiceModel)}");
                    return await base.InsertOneAsync(repairInvoiceModel);
                }
                else
                {
                    throw new Exception($"SaveOnPremAsync - Failure { await response.Content?.ReadAsStringAsync()}");
                }
                
            }
            catch (Exception ex)
            {               
                _logger.LogError($"Repair Invoice with error{ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> SaveOnPremAsync(RepairInvoiceModel model,JwtInfo jwtInfo)
        {
            try
            {
                _logger.LogInformation($"Invoke SaveOnPremAsync {JsonConvert.SerializeObject(model)}");              

                string token = (jwtInfo != null) ? jwtInfo.Token : null;              
                _logger.LogInformation($"Invoke SaveOnPremAsync - Token {token}");
                return await _httpService.PostAsync(_ttcServiceConfig.RepairInvoiceEndPoint,
                               _ttcServiceConfig.RepairInvoiceEndPoint, token,
                               AppConstants.HttpContentTypeJson,
                               JsonConvert.SerializeObject(model),
                               CancellationToken.None);

                
            }
            catch 
            {
                throw;
            }
        }

        public override async Task<RepairInvoiceModel> ReplaceOneAsync(RepairInvoiceModel model)
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

        public async Task<RepairInvoiceModel> FindOneAsync(int orderId,string AccountId)
        {
            return await base.FindOneAsync(model => model.OrderId == orderId && model.AccountId == AccountId);
        }
    }
}
