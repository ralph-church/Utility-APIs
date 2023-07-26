using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.constants;
using repair.service.service.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using repair.service.shared.paging;

namespace repair.service.service
{
    public class RepairRequestService : BaseService<RepairRequestModel, RepairRequest>,
        IRepairRequestService
    {
        private readonly ILogger<RepairRequestService> _logger;
        private readonly IDataRepository<RepairRequest> _repository;
        private readonly IMapper _mapper;

        public RepairRequestService(ILogger<RepairRequestService> logger,
                                   IDataRepository<RepairRequest> repository,
                                   IMapper mapper) : base(repository, mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<RepairRequestModel> InsertOneAsync(RepairRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke InsertOneAsync {JsonConvert.SerializeObject(model)}");
                var repairRequestModel = await base.InsertOneAsync(model);
                return repairRequestModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Repair services with error{ex.Message}");
                throw;
            }
        }


        public override async Task<RepairRequestModel> ReplaceOneAsync(RepairRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Invoke ReplaceOneAsync {JsonConvert.SerializeObject(model)}");
                var repairRequestModel = await base.ReplaceOneAsync(model);
                return repairRequestModel;
            }
            catch (Exception ex)
            {
                string payload = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Repair services with error{ex.Message}");
                throw;
            }
        }

        public async Task<RepairRequestModel> FindOneAsync(string accountId, string instanceId, string orderId, string requestNumber, bool createVRO)
        {
            //For TMT VRO flow, the orderid will be zero
            if (createVRO)
            {
                return await base.FindOneAsync(model => model.AccountId == accountId &&
                                                        model.IntegrationName == instanceId &&
                                                        model.Details.RequestNumber == requestNumber);
            }
            else
            {
                return await base.FindOneAsync(model => model.AccountId == accountId &&
                                                        model.IntegrationName == instanceId &&
                                                        model.OrderId == orderId);
            }
        }

        public async Task<RepairRequestModel> ReplaceOneAsync(RepairRequestModel existingModel, RepairRequestModel modelToUpdate)
        {
            var updatedModel = UpdateModel(existingModel, modelToUpdate);
            return await base.ReplaceOneAsync(updatedModel);
        }

        private static RepairRequestModel UpdateModel(RepairRequestModel existingModel, RepairRequestModel modelToUpdate)
        {
            //Update RepairRequest
            existingModel.SendToExternalSystem = modelToUpdate.SendToExternalSystem;
            existingModel.IsSentToExternalSystem = modelToUpdate.IsSentToExternalSystem;
            existingModel.ModifiedBy = modelToUpdate.ModifiedBy;
            existingModel.ModifiedDate = DateTime.Now;            

            //Update Details
            existingModel.Details.RequestNumber = modelToUpdate.Details.RequestNumber;
            existingModel.Details.RequestStatus = modelToUpdate.Details.RequestStatus;
            existingModel.Details.Vendor = modelToUpdate.Details.Vendor;
            existingModel.Details.Vin = modelToUpdate.Details.Vin;
            existingModel.Details.Customer = modelToUpdate.Details.Customer;
            existingModel.Details.Shop = modelToUpdate.Details.Shop;
            existingModel.Details.ETA = modelToUpdate.Details.ETA;
            existingModel.Details.IsActionRequired = modelToUpdate.Details.IsActionRequired;
            if (modelToUpdate.Details.ROETC != null)
            {
                existingModel.Details.ROETC = modelToUpdate.Details.ROETC;
            }
            else
            {
                existingModel.Details.ROETC = null;
            }
            existingModel.Details.InstanceId = modelToUpdate.Details.InstanceId;
            existingModel.Details.ObjId = modelToUpdate.Details.ObjId;
            existingModel.Details.ObjType = modelToUpdate.Details.ObjType;

            //Update Comments
            //Fetch only saved comments (issubmited == false) from existingModel in db
            var savedComments = existingModel.Details.Comments.Where(i => !i.IsSubmitted && string.IsNullOrEmpty(i.CommentId)).ToList();
            if (savedComments != null)
            {
                //Merge new or updated list of comments with the existing savedComments
                savedComments.AddRange(modelToUpdate.Details.Comments.ToList());

                //Replace existing comments with merged comments
                existingModel.Details.Comments = savedComments?.ToArray();
            }
            else
            {
                existingModel.Details.Comments = modelToUpdate.Details.Comments;
            }


            //Update Sections
            //Fetch only saved sections (issubmited == false) from existingModel in db
            var savedSections = existingModel.Details.Sections.Where(i => !i.IsSubmitted).ToList();
            if (savedSections != null)
            {
                //Merge new or updated list of sections with the existing savedComments
                savedSections.AddRange(modelToUpdate.Details.Sections.ToList());

                //Replace existing comments with merged comments
                existingModel.Details.Sections = savedSections?.ToArray();
            }
            else
            {
                existingModel.Details.Sections = modelToUpdate.Details.Sections;
            }
            return existingModel;
        }

        /// <summary>
        /// Method to update repair request number in repair_request collection
        /// </summary>
        /// <param name="repairRequestStage"></param>
        /// <returns></returns>

        public async Task UpdateRequestNumberInRepairRequest(RepairRequestStageModel repairRequestStage)
        {
            if (repairRequestStage != null &&
                repairRequestStage.CallerActionType.ToUpper() == RepairServiceConstants.CreateSeriveRequest &&
                !string.IsNullOrEmpty(repairRequestStage.AccountId) &&
                !string.IsNullOrEmpty(repairRequestStage.ReferenceNumber) &&
                repairRequestStage.ResponseData != null)
            {
                var responseDataDeserialized = (JObject)JsonConvert.DeserializeObject(repairRequestStage.ResponseData.ToString());
                responseDataDeserialized.TryGetValue("SRId", out JToken sridAsObject);
                var sridAsValue = sridAsObject != null ? sridAsObject.Value<string>() : string.Empty;


                if (!string.IsNullOrEmpty(sridAsValue))
                {
                    var existingRepairRequest = await FindOneAsync(x =>
                        x.AccountId == repairRequestStage.AccountId
                        && x.OrderId == repairRequestStage.ReferenceNumber);

                    if (existingRepairRequest != null && existingRepairRequest.Details != null)
                    {
                        existingRepairRequest.Details.RequestNumber = sridAsValue;
                        var isUpdated = await UpdateAsync(existingRepairRequest);
                        if (isUpdated)
                        {
                            _logger.LogInformation($"RepairRequestStages - Post-Model - Reference number updated in repair request : {JsonConvert.SerializeObject(repairRequestStage)}");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"RepairRequestStages - Post-Model - Reference number is not crerated  : {JsonConvert.SerializeObject(repairRequestStage)}");
                }
            }
        }

        public async Task<PagedResponseList<RepairRequestModel>> GetRepairRequests(string accountId, QueryStringParams queryStringParams)
        {
            PagedResponseList<RepairRequest> repairRequests = await _repository.FilterByQueryParams(accountId, queryStringParams);
            PagedResponseList<RepairRequestModel> repairRequestModels =
                                _mapper.Map<PagedResponseList<RepairRequest>, PagedResponseList<RepairRequestModel>>(repairRequests);
            return await Task.FromResult(repairRequestModels);
        }
    }
}