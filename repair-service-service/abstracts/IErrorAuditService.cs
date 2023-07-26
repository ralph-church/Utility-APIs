using repair.service.data.entities;
using repair.service.service.model;
using System;

namespace repair.service.service.abstracts
{
    public interface IErrorAuditService : IBaseService<ErrorAuditModel, ErrorAudit>
    {        
        public bool isValidErrorAudit(ErrorAuditModel errorAudit);
        public ErrorAuditModel CreateErrorModel(string accountId, string eventId, string eventName, string payLoadException, Exception exceptionModel = null, string routingGatewayRegistryId = null);
    }
}