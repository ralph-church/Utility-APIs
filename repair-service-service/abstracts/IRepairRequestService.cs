using repair.service.data.entities;
using repair.service.service.model;
using repair.service.shared.model;
using repair.service.shared.paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IRepairRequestService : IBaseService<RepairRequestModel, RepairRequest>
    {
        Task<RepairRequestModel> FindOneAsync(string accountId, string instanceId, string orderId, string requestNumber, bool createVRO);

        Task<RepairRequestModel> ReplaceOneAsync(RepairRequestModel existingModel, RepairRequestModel modelToUpdate);
        Task UpdateRequestNumberInRepairRequest(RepairRequestStageModel repairRequestStage);
        Task<PagedResponseList<RepairRequestModel>> GetRepairRequests(string accountId, QueryStringParams queryStringParams);

    }
}
    