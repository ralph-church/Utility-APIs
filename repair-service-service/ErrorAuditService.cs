using AutoMapper;
using Microsoft.Extensions.Logging;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using System;

namespace repair.service.service
{
    public class ErrorAuditService : BaseService<ErrorAuditModel, ErrorAudit>,
        IErrorAuditService
    {

        private readonly ILogger<ErrorAuditService> _logger;

        #region Constructor

        public ErrorAuditService(
            ILogger<ErrorAuditService> logger,
            IDataRepository<ErrorAudit> repository,
            IMapper mapper) : base(repository, mapper)
        {

            _logger = logger;
        }
        #endregion

        public bool isValidErrorAudit(ErrorAuditModel errorAudit)
        {
            try
            {
                return errorAudit != null && !string.IsNullOrEmpty(errorAudit.EventId)
                && !string.IsNullOrEmpty(errorAudit.ErrorState)
                && !string.IsNullOrEmpty(errorAudit.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured {ex.Message} ");
                throw ex;
            }

        }
        
        public ErrorAuditModel CreateErrorModel(string accountId, string eventId, string eventName, string payLoad, Exception exceptionModel = null, string routingGatewayRegistryId = null)
        {
            try
            {
                ErrorAuditModel errorAuditModel = null;

                errorAuditModel = new ErrorAuditModel
                {
                    EventId = eventId,
                    //EventType = sourceModel.MessageType,
                    EventSource = accountId,
                    EventSubject = eventName,
                    Payload = payLoad,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                };

                if (exceptionModel != null)
                {
                    errorAuditModel.RoutingGatewayRegistryId = string.IsNullOrEmpty(routingGatewayRegistryId) ? string.Empty : routingGatewayRegistryId;
                    errorAuditModel.ErrorMessage = exceptionModel.Message;
                }
                return errorAuditModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured {ex.Message} ");
                throw ex;
            }
        }
    }
}
