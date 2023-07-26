using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using repair.service.api.controllers;
using repair.service.service.abstracts;
using repair.service.service.constants;
using repair.service.service.model;
using repair.service.shared.exception.model;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace repair.service.api.Controllers
{
    [Route("maintenance/v{version:apiVersion}/repair/request-stages")]
    [ApiVersion("1.0")]
    public class RepairRequestStageController : RepairServiceControllerBase
    {
        private readonly ILogger<RepairRequestStageController> _logger;
        private readonly IRepairRequestStageService _repairRequestStagesService;
        private readonly IRepairRequestService _repairRequestService;
        private BadRequestObjectResult _badRequestObjectResult;
        private NotFoundObjectResult _notFoundObjectResult;

        public RepairRequestStageController(IRepairRequestStageService repairRequestStagesService,
                                            IRepairRequestService repairRequestService,
                                             ILogger<RepairRequestStageController> logger)
        {
            _repairRequestStagesService = repairRequestStagesService ?? throw new ArgumentNullException(nameof(repairRequestStagesService));
            _repairRequestService = repairRequestService ?? throw new ArgumentNullException(nameof(repairRequestService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }



        [HttpGet("{id}", Name = "GetRepairRequestStageById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
                 Summary = "Get RepairRequest by Id",
                 Description = "Get RepairRequest based on give input")
                ]
        public async Task<ActionResult<RepairRequestStageModel>> GetById(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairRequestStage = await _repairRequestStagesService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairRequestStage, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            return Ok(repairRequestStage);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairRequestStageModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
             Summary = "Get Repair Request Stages",
             Description = "Get Repair request stages based on given input")
        ]
        public async Task<ActionResult<RepairRequestStageModel>> GetRepairRequestStages(string accountId, string instanceId, string referenceNumber, string eventId)
        {
            //This method will do polling until the search is successful upto 1 minute
            //Temporary solution until we implement signalR notification
            //Search for the status of repair request and send the response back to the client
            if (base.IsBadRequest(accountId, referenceNumber, instanceId, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            List<RepairRequestStageModel> model = null;
            // the delay frequency keeps decreasing as the time increases the chances are very high to get the response
            // This method takes 40 seconds time to find document .In case it finds document before 40 seconds it returns document otherwise return not found status.
            int[] delayFrequencies = { 10, 5, 5, 5, 5, 5, 5 }; 
            int iteration = 0;
            int count = delayFrequencies.Count();
            while (iteration < count)
            {
                if (iteration > 0)
                {
                    if (eventId == null)
                    {
                        model = (await _repairRequestStagesService.FilterBy((model => model.AccountId == accountId && model.InstanceId == instanceId
                                                           && model.ReferenceNumber == referenceNumber && model.EventId == eventId), null, null)).ToList();
                    }
                    else
                    {
                        model = (await _repairRequestStagesService.FilterBy((model => model.AccountId == accountId
                                                        && model.ReferenceNumber == referenceNumber), null, null)).ToList();
                    }
                }

                if (model == null || model.Count == 0)
                    await Task.Delay(delayFrequencies[iteration] * 1000);

                iteration++;
            }

            if (model == null || model.Count == 0)
                return NotFound(new ErrorModel() { StatusCode = 404, Message = "The requested resource not found" });
            else
                return Ok(model);
        }


        [HttpPost]
        [ProducesResponseType(typeof(RepairRequestStageModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
               Summary = "Post method",
               Description = "Post method")
           ]
        public async Task<ActionResult<object>> PostAsync([FromBody] RepairRequestStageModel repairRequestStageModel)
        {
            if (IsBadRequest(repairRequestStageModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            if (repairRequestStageModel.CreatedDate == null)
            {
                repairRequestStageModel.CreatedDate = DateTime.Now;
            }

            using (LogContext.PushProperty("CorrId", repairRequestStageModel.EventId))
            {
                _logger.LogInformation($"RepairRequestStages - Post-Model  : {JsonConvert.SerializeObject(repairRequestStageModel)}");
                await _repairRequestService.UpdateRequestNumberInRepairRequest(repairRequestStageModel);
                var result = await _repairRequestStagesService.InsertOneAsync(repairRequestStageModel);               
                return CreatedAtRoute("GetRepairRequestStageById", new { id = result.Id }, result);
            }
        }
    }
}
