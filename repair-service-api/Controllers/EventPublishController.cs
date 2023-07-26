using Datadog.Trace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.exception.model;
using Serilog.Context;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace repair.service.api.controllers
{
    [Route("maintenance/v{version:apiVersion}/event/publish")]
    [ApiVersion("1.0")]
    public class EventPublishController : RepairServiceControllerBase
    {
        private readonly ILogger<EventPublishController> _logger;
        private readonly IOutboundService _outboundService;
        private BadRequestObjectResult _badRequestObjectResult;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventPublishController(IOutboundService outboundService, ILogger<EventPublishController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _outboundService = outboundService ?? throw new ArgumentNullException(nameof(outboundService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventPublishModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
            Summary = "Post method",
            Description = "Post method")
        ]
        public async Task<ActionResult<object>> PostAsync([FromBody] EventPublishModel dto)
        {
            var scope = Tracer.Instance.StartActive("Event Publish");
            if (IsBadRequest(dto, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            using (LogContext.PushProperty("CorrId", dto.EventId))
            {
                try
                {
                    // Access the active scope through
                    // the global tracer (can return null)
                 
                    if (scope != null && scope.Span!=null)
                    {
                        // Add a tag to the span for use in the Datadog web UI
                        scope.Span.ResourceName = "Event Publish";
                        scope.Span.SetTag(Tags.SpanKind, SpanKinds.Producer);
                        scope.Span.SetTag("Event Publish", dto.EventId);
                        _logger.LogInformation("Message during a trace.");
                    }
                    else
                    {
                        _logger.LogInformation("Scope or Span is empty");
                    }
                   

                  
                    dto.EventSource = new Uri( _httpContextAccessor.HttpContext.Request.GetEncodedUrl());
                    var response = await _outboundService.PublishAsync(dto);
                    return response;
                }   
                catch (Exception e)
                {
                    scope?.Span?.SetException(e);
                    ErrorModel error = new ErrorModel() { StatusCode = (int)HttpStatusCode.BadRequest, Message = e.Message, Details = e.StackTrace };
                    return BadRequest(error);
                }
            }
        }      
    }
}


