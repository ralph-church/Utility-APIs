using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using repair.service.api.controllers;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.service.model;
using repair.service.shared.abstracts;
using repair.service.shared.exception.model;
using repair.service.shared.model;
using repair.service.shared.paging;
using repair.service.shared.constants;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace repair.service.api.Controllers
{
    [Route("maintenance/v{version:apiVersion}/repair/requests")]
    [ApiVersion("1.0")]
    public class RepairRequestController : RepairServiceControllerBase
    {
        private readonly ILogger<RepairRequestController> _logger;
        private readonly IRepairRequestService _repairRequestService;
        private readonly IRepairInvoiceService _repairInvoiceService;
        private BadRequestObjectResult _badRequestObjectResult;        
        private NotFoundObjectResult _notFoundObjectResult;
        private IAuditService _auditService;



        public RepairRequestController(IRepairRequestService repairRequestService, IRepairInvoiceService repairInvoiceService, ILogger<RepairRequestController> logger, IAuditService auditService)
        {
            _repairRequestService = repairRequestService ?? throw new ArgumentNullException(nameof(repairRequestService));
            _repairInvoiceService = repairInvoiceService ?? throw new ArgumentNullException(nameof(repairInvoiceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auditService = auditService;
        }

        [HttpGet("{id}", Name = "GetRepairRequestById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
           Summary = "Get Repair Request by Id",
           Description = "Get Repair Request based on give input")
       ]
        public async Task<ActionResult<RepairRequestModel>> GetById(string id)
        {
            if (base.IsBadRequest(id, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            var repairRequest = await _repairRequestService.FindByIdAsync(id);
            if (base.IsEntityNotFound(id, repairRequest, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            return Ok(repairRequest);
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
             Summary = "Get Repair Request",
             Description = "Get Repair Request By accountId,instanceId,orderId")
         ]
        public async Task<ActionResult<object>> GetAsync(string accountId, string instanceId, string orderId)
        {

            if (IsBadRequestWithNullValues(accountId, instanceId, orderId, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }
            try
            {
                _logger.LogInformation($"Get Async - Received parameters - Instance Id - {instanceId},Account Id - {accountId},Order Id - {orderId} ");
                var result = await _repairRequestService.FindOneAsync(accountId, instanceId, orderId,"",false);
                if (result != null && result.ModifiedDate != null)
                {
                    var ETag = CreateMD5(result.ModifiedDate.ToString());
                    Response?.Headers.Add("If-Match", ETag);
                }
                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error occurred in GetAsync with params =====> ", accountId, orderId, instanceId);                
                return Content(e.Message);
            }
        }

        [HttpGet("list", Name = "GetRepairRequests")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PagedResponseList<RepairRequestModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
             Summary = "Get List Of Repair Requests",
             Description = "Get List Of Repair Requests By accountId,searchParams")
         ]
        public async Task<ActionResult<RepairRequestModel>> GetAllByQueryParamsAsync(string accountId, [FromQuery] QueryStringParams queryStringParams)
        {
            try
            {
                if (IsBadRequest(accountId, out _badRequestObjectResult))
                {
                    return _badRequestObjectResult;
                }

                _logger.LogDebug($"Get All By QueryParams - Received parameters - Account Id - {accountId}");

                if(queryStringParams != null && queryStringParams.Filter != null)
                {
                    queryStringParams.Filter.ForEach(delegate(Filtering filtering) {
                        _logger.LogDebug($"Search parameter - Field Name - {filtering.Field},Operator - {filtering.Operator},Value - {filtering.Value}");
                    });
                }

                //Due to automatic modal binding, added code to validate null for queryStringParams
                //Below validation might be required if paging is supported, at the moment paging keys are not required for the Repair Requests.
                //queryStringParams = Request != null ? (base.IsValidPaging(Request.Query.Keys) ? queryStringParams : null) : queryStringParams;
                var repairRequestModels = await _repairRequestService.GetRepairRequests(accountId, queryStringParams);
                if (repairRequestModels != null && base.IsEntityNotFound(accountId, repairRequestModels.Data, out _notFoundObjectResult))
                {
                    return _notFoundObjectResult;
                }
                return Ok(repairRequestModels);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred in GetAllByQueryParamsAsync with params =====> ", accountId);
                return Content(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.Created)]        
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
           Summary = "Post method",
           Description = "Post method")
       ]
        public async Task<ActionResult<RepairRequestModel>> PostAsync([FromBody] RepairRequestModel repairRequestModel)
        {
            if (IsBadRequest(repairRequestModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Post Async - Received RepairRequestModel {JsonConvert.SerializeObject(repairRequestModel)} ");

            if (repairRequestModel.CreatedDate == null)
            {
                repairRequestModel.CreatedDate = DateTime.Now;
            }


            RepairRequestModel result = await _repairRequestService.InsertOneAsync(repairRequestModel);
            return CreatedAtRoute("GetRepairRequestById", new { id = result.Id }, result);               
        }

        
        [HttpPut]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
                Summary = "Put method",
                Description = "Put method")
            ]
        public async Task<ActionResult<object>> PutAsync([FromBody] RepairRequestModel repairRequestModel)
        {
            if (IsBadRequest(repairRequestModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Put Async - Received RepairRequestModel {JsonConvert.SerializeObject(repairRequestModel)} ");           
        
            var existingModel = await _repairRequestService.FindOneAsync(repairRequestModel.AccountId,
                                                                        repairRequestModel.IntegrationName,
                                                                        repairRequestModel.OrderId,
                                                                        repairRequestModel.Details.RequestNumber,
                                                                        repairRequestModel.CreateVRO);            

            if (base.IsEntityNotFound(repairRequestModel.AccountId, repairRequestModel.IntegrationName, repairRequestModel.OrderId,existingModel, out _notFoundObjectResult))
            {
                return _notFoundObjectResult;
            }
            
            var existingModelETag = CreateMD5(existingModel.ModifiedDate.ToString());
            if (Request != null)
            {
                if (Request.Headers.ContainsKey("If-Match") && Request.Headers["If-Match"] != existingModelETag)
                {
                    return StatusCode(412, "The data on this page is out of date, please refresh the page.");
                }
            }

            var jwtToken = GetToken();
            //CreateVRO is true for i360 ISA triggered Create VRO flow from scheduler  
            if (repairRequestModel.CreateVRO && Convert.ToInt32(existingModel.OrderId) <= 0)
            { 
                var response = await _repairInvoiceService.SaveOnPremAsync(repairRequestModel.RepairInvoice, jwtToken);
                if (response.IsSuccessStatusCode)
                {
                    var responseDataDeserialized = (JObject)JsonConvert.DeserializeObject(await response.Content?.ReadAsStringAsync());
                    _logger.LogInformation($"Invoke SaveOnPremAsync - Success {responseDataDeserialized}");
                    responseDataDeserialized.TryGetValue("orderId", out JToken orderId);
                    responseDataDeserialized.TryGetValue("objectId", out JToken objectId);
                    responseDataDeserialized.TryGetValue("objectType", out JToken objectType);
                    existingModel.CreateVRO = false;
                    existingModel.OrderId = Convert.ToString(orderId);
                    repairRequestModel.Details.ObjId  = Convert.ToString(objectId);
                    repairRequestModel.Details.ObjType = Convert.ToString(objectType);
                }
                else
                {
                    _logger.LogInformation($"Invoke SaveOnPremAsync - Failure {await response.Content?.ReadAsStringAsync()}");
                }
            }

            if (repairRequestModel.Details.RequestStatus.ToUpper() == "DELIVERED")
            {
                await _auditService.PostAuditAsync(repairRequestModel, jwtToken);
            }

            //Invoke repair invoice service to update
            await _repairRequestService.ReplaceOneAsync(existingModel, repairRequestModel);
            return NoContent();
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
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
