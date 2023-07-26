using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.api.controllers;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.exception.model;
using repair.service.shared.model;
using repair.service.shared.constants;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace repair.service.api.Controllers
{
    [Route("maintenance/v{version:apiVersion}/repair/invoices")]
    [ApiVersion("1.0")]
    public class RepairInvoiceController : RepairServiceControllerBase
    {
        private readonly ILogger<RepairInvoiceController> _logger;
        private readonly IRepairInvoiceService _repairInvoiceService;
        private BadRequestObjectResult _badRequestObjectResult;
        private NotFoundObjectResult _notFoundObjectResult;
        public RepairInvoiceController(IRepairInvoiceService repairRequestService, ILogger<RepairInvoiceController> logger)
        {
            _repairInvoiceService = repairRequestService ?? throw new ArgumentNullException(nameof(repairRequestService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{OrderId},{AccountId}", Name = "GetRepairInvoiceByOrderId")]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Get Repair Invoice By OrderId and AccountId",
            Description = "Get Repair Invoice based on given input")
           ]
        public async Task<ActionResult<RepairInvoiceModel>> GetRepairInvoiceOrder(int OrderId, string AccountId)
        {
            if (base.IsBadRequest(OrderId, AccountId, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Get Async - Received parameters - Account Id - {AccountId},Order Id - {OrderId} ");
            var result = await _repairInvoiceService.FindOneAsync(OrderId, AccountId);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "List Repair Invoice Model",
            Description = "List Repair Invoice Model")
           ]
        public async Task<ActionResult<RepairInvoiceModel>> GetAll()
        {

            var model = await _repairInvoiceService.FilterBy(x => true, null,null);
            return Ok(model);

        }

        [HttpGet("{id}", Name = "GetRepairInvoiceById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
           Summary = "Get Repair Invoice by Id",
           Description = "Get Repair Invoice based on give input")
       ]
        public async Task<ActionResult<RepairInvoiceModel>> GetById(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairRequest = await _repairInvoiceService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairRequest, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            return Ok(repairRequest);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
          Summary = "Post method",
          Description = "Post method")
            ]
        public async Task<ActionResult<RepairInvoiceModel>> PostAsync([FromBody] RepairInvoiceModel repairinvoiceModel)
        {
            if (IsBadRequest(repairinvoiceModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            if (repairinvoiceModel.CreatedDate == null)
            {
                repairinvoiceModel.CreatedDate = DateTime.Now;
            }
            RepairInvoiceModel result = await _repairInvoiceService.InsertOneAsync(repairinvoiceModel, GetToken());
            return CreatedAtRoute("GetRepairInvoiceById", new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RepairInvoiceModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
                    Summary = "Update Repair Invoice Model",
                    Description = "Update Repair Invoice Model")
            ]
        public async Task<ActionResult<RepairInvoiceModel>> Update([FromBody] RepairInvoiceModel repairinvoiceModel)
        {
            if (IsBadRequest(repairinvoiceModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Put Async - Received Repair Invoice Model {JsonConvert.SerializeObject(repairinvoiceModel)} ");

            if (repairinvoiceModel.ModifiedDate == null)
            {
                repairinvoiceModel.ModifiedDate = DateTime.Now;
            }

            var isUpdated = await _repairInvoiceService.UpdateAsync(repairinvoiceModel);
            if (isUpdated)
            {
                return CreatedAtRoute("GetRepairInvoiceById", new { id = repairinvoiceModel.Id }, repairinvoiceModel);
            }
            IsEntityNotFound(repairinvoiceModel.Id, entityModel: null, out _notFoundObjectResult);
            return _notFoundObjectResult;

        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete Repair Invoice",
            Description = "Delete Repair Invoice")
            ]
        public async Task<ActionResult> Delete(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairinvoiceToDelete = await _repairInvoiceService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairinvoiceToDelete, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            await _repairInvoiceService.DeleteByIdAsync(id);
            return NoContent();
        }

        private JwtInfo GetToken()
        {
            if (HttpContext == null)
                return null;

            var jwtinfo = (JwtInfo)HttpContext.Items[AppConstants.JwtInfo];
            return jwtinfo;
        }

    }
}

