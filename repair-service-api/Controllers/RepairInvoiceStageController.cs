using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.api.controllers;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.exception.model;
using repair.service.shared.paging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace repair.service.api.Controllers
{
    [Route("maintenance/v{version:apiVersion}/repair/invoice-stages")]
    [ApiVersion("1.0")]
    public class RepairInvoiceStageController : RepairServiceControllerBase
    {
        private readonly ILogger<RepairInvoiceStageController> _logger;
        private readonly IRepairInvoiceStageService _repairInvoiceStageService;
        private BadRequestObjectResult _badRequestObjectResult;
        private NotFoundObjectResult _notFoundObjectResult;
        public RepairInvoiceStageController(IRepairInvoiceStageService repairInvoiceStageService, ILogger<RepairInvoiceStageController> logger)
        {
            _repairInvoiceStageService = repairInvoiceStageService ?? throw new ArgumentNullException(nameof(repairInvoiceStageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet("{id}", Name = "GetRepairInvoiceStageById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
           Summary = "Get Repair Invoice Stage by Id",
           Description = "Get Repair Invoice based Stage on give input")
        ]
        public async Task<ActionResult<RepairInvoiceStageModel>> GetByIdAsync(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairInvocieStage = await _repairInvoiceStageService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairInvocieStage, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            return Ok(repairInvocieStage);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseList<RepairInvoiceStageModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "List Repair Invoice Stages",
            Description = "List Repair Invoice Stages")
        ]
        public async Task<ActionResult<PagedResponseList<RepairInvoiceStageModel>>> GetAllAsync(string groupKey, int maxRetryCount,
                                                                                      [FromQuery] QueryStringParams queryStringParams)
        {
            //Due to automatic modal binding, added code to validate null for queryStringParams
            queryStringParams = Request != null ? (base.IsValidPaging(Request.Query.Keys) ? queryStringParams : null) : queryStringParams;
            var repairinvoiceStageModels = await _repairInvoiceStageService.FindAsync(groupKey, maxRetryCount, queryStringParams);
            return Ok(repairinvoiceStageModels);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RepairInvoiceStageModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
          Summary = "Post method",
          Description = "Post method")
            ]
        public async Task<ActionResult<RepairInvoiceStageModel>> PostAsync([FromBody] RepairInvoiceStageModel repairinvoiceStageModel)
        {
            if (base.IsBadRequest(repairinvoiceStageModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            if (repairinvoiceStageModel.CreatedDate == null)
            {
                repairinvoiceStageModel.CreatedDate = DateTime.Now;
            }
            RepairInvoiceStageModel result = await _repairInvoiceStageService.InsertOneAsync(repairinvoiceStageModel);
            return CreatedAtRoute("GetRepairInvoiceStageById", new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RepairInvoiceStageModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
                    Summary = "Update Repair Invoice Stage Model",
                    Description = "Update Repair Invoice Stage Model")
            ]
        public async Task<ActionResult<RepairInvoiceStageModel>> Update([FromBody] RepairInvoiceStageModel repairInvoiceStageModel)
        {
            if (IsBadRequest(repairInvoiceStageModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Put Async - Received Repair Invoice Stage Model {JsonConvert.SerializeObject(repairInvoiceStageModel)} ");

            if (repairInvoiceStageModel.ModifiedDate == null)
            {
                repairInvoiceStageModel.ModifiedDate = DateTime.Now;
            }

            var isUpdated = await _repairInvoiceStageService.UpdateAsync(repairInvoiceStageModel);
            if (isUpdated)
            {
                return CreatedAtRoute("GetRepairInvoiceStageById", new { id = repairInvoiceStageModel.Id }, repairInvoiceStageModel);
            }
            IsEntityNotFound(repairInvoiceStageModel.Id, entityModel: null, out _notFoundObjectResult);
            return _notFoundObjectResult;

        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete Repair Invoice Stage",
            Description = "Delete Repair Invoice Stage")
            ]
        public async Task<ActionResult> Delete(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairInvoiceStageToDelete = await _repairInvoiceStageService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairInvoiceStageToDelete, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            await _repairInvoiceStageService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
