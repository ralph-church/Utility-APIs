using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repair.service.api.controllers;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.exception.model;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace repair.service.api.Controllers
{
    [Route("maintenance/v{version:apiVersion}/error/audits")]
    [ApiVersion("1.0")]
    public class ErrorAuditController : RepairServiceControllerBase
    {
        private readonly IErrorAuditService _errorAuditService;
        private readonly ILogger<ErrorAuditController> _logger;

        private BadRequestObjectResult _badRequestObjectResult;
        private NotFoundObjectResult _notFoundObjectResult;

        public ErrorAuditController(IErrorAuditService errorAuditService, ILogger<ErrorAuditController> logger)
        {
            _errorAuditService = errorAuditService ?? throw new ArgumentNullException(nameof(errorAuditService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorAuditModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "List Error Audit",
            Description = "List Error Audit")
        ]
        public async Task<ActionResult<ErrorAuditModel>> GetAll()
        {
            var errorAudits = await _errorAuditService.FilterBy(x => true, null, null);
            return Ok(errorAudits);
        }

        [HttpGet("{id}", Name = "GetErrorAuditById")]
        [ProducesResponseType(typeof(ErrorAuditModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Get Error Audit",
            Description = "Get Error Audit based on give input")
        ]
        public async Task<ActionResult<ErrorAuditModel>> GetById(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            var errorAudit = await _errorAuditService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, errorAudit, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            return Ok(errorAudit);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorAuditModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Create Error Audit",
            Description = "Create Error Audit")
        ]
        public async Task<ActionResult<ErrorAuditModel>> Create([FromBody] ErrorAuditModel errorAuditModel)
        {
            if (base.IsBadRequest(errorAuditModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            if (_errorAuditService.isValidErrorAudit(errorAuditModel))
            {
                var errorAuditModelFromDb = await _errorAuditService.InsertOneAsync(errorAuditModel);
                return CreatedAtRoute("GetErrorAuditById", new { id = errorAuditModelFromDb.Id }, errorAuditModelFromDb);
            }
            return _badRequestObjectResult;

        }

        [HttpPut]
        [ProducesResponseType(typeof(ErrorAuditModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Update Error Audit",
            Description = "Update Error Audit")
        ]
        public async Task<ActionResult<ErrorAuditModel>> Update([FromBody] ErrorAuditModel errorAuditModel)
        {
            if (IsBadRequest(errorAuditModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            var updatedErrorAuditModelFromDb = await _errorAuditService.ReplaceOneAsync(errorAuditModel);
            if (updatedErrorAuditModelFromDb != null)
            {
                return CreatedAtRoute("GetErrorAuditById", new { id = updatedErrorAuditModelFromDb.Id }, updatedErrorAuditModelFromDb);
            }
            IsEntityNotFound(updatedErrorAuditModelFromDb.Id, entityModel: null, out _notFoundObjectResult);
            return _notFoundObjectResult;
            
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete Error Audit",
            Description = "Delete Error Audit")
        ]
        public async Task<ActionResult> Delete(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var errorAuditToDelete = await _errorAuditService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, errorAuditToDelete, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            await _errorAuditService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}