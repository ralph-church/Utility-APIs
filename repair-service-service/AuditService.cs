using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.abstracts;
using repair.service.shared.model;
using repair.service.shared.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace repair.service.service
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;
        private readonly IHttpService _httpService;
        private readonly ITtcServicesConfig _ttcServicesConfig;
        private readonly IConfiguration _configuration;

        public AuditService(
          ILogger<AuditService> logger,
          IHttpService httpService, 
          ITtcServicesConfig ttcServicesConfig,
          IServiceProvider serviceProvider
         ) 
        {
            _logger = logger;
            _httpService = httpService;
            _ttcServicesConfig = ttcServicesConfig;
             _configuration = serviceProvider?.GetRequiredService<IConfiguration>();
        }

        public async Task<HttpResponseMessage> PostAuditAsync(RepairRequestModel repairRequestModel, JwtInfo jwtInfo)
        {
            var token = jwtInfo?.Token;
            _logger.LogInformation($"Aduit URL :{_ttcServicesConfig.AuditEndPoint}");
            var response = await _httpService.PostAsync(_ttcServicesConfig.AuditEndPoint, _ttcServicesConfig.AuditEndPoint, token, AppConstants.HttpContentTypeJson,
                              JsonConvert.SerializeObject(GetAuditPayLoad(repairRequestModel, jwtInfo)), CancellationToken.None);
            return response;
        }

        private object GetAuditPayLoad(RepairRequestModel repairRequestModel, JwtInfo jwtInfo)
        {
            var references = new
            {
                source = "Repair Service",
                caller = repairRequestModel.IntegrationName,
                tenantName = jwtInfo?.AccountName,
                refId = repairRequestModel.Details.RequestNumber,
                refNumber = repairRequestModel.OrderId,
                businessUnit = "TMT",
                messageType = repairRequestModel.IntegrationType
            };

            var auditEventType = _configuration?.GetSection("AuditEventType")?.Value;

            var auditPayLoad = new
            {
                auditId = Guid.NewGuid().ToString(),
                correlationId = Guid.NewGuid().ToString(),
                source = Environment.GetEnvironmentVariable("CANONICALNAME") ?? AppConstants.CANONICAL_NAME,
                eventType = auditEventType ?? AppConstants.AUDIT_EVENT_TYPE,
                description = "Audit for Repair Request",
                resultEnum = 2,
                references = references,
                sourceAccountId = repairRequestModel.AccountId,
                destinationAccountId = repairRequestModel.AccountId,
            };

            _logger.LogInformation($"Aduit CorrelationId :{JsonConvert.SerializeObject(auditPayLoad.correlationId)}");
            return auditPayLoad;

        }
    }
}
