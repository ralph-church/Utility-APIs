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
    [Route("maintenance/v{version:apiVersion}/notifications")]
    [ApiVersion("1.0")]
    public class NotificationServiceController : NotificationsServiceControllerBase
    {
        private readonly ILogger<NotificationServiceController> _logger;
        private readonly INotificationService _notificationService;
        private BadRequestObjectResult _badRequestObjectResult;        
        private NotFoundObjectResult _notFoundObjectResult;
        private IAuditService _auditService;



        public NotificationServiceController(INotificationService notificationService, ILogger<NotificationServiceController> logger, IAuditService auditService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _auditService = auditService;
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(RepairRequestModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
                Summary = "Put method",
                Description = "Put method")
            ]
        public async Task<ActionResult<object>> PutAsync([FromBody] NotificationModel notificationModel)
        {
            if (IsBadRequest(repairRequestModel, out _badRequestObjectResult))
            {
                return _badRequestObjectResult;
            }

            _logger.LogInformation($"Put Async - Received NotificationModel {JsonConvert.SerializeObject(notificationModel)} ");           
        
            //var existingModel = await _repairRequestService.FindOneAsync(notificationModel.LocationWorkOrder);            

            //if (base.IsEntityNotFound(repairRequestModel.AccountId, repairRequestModel.IntegrationName, repairRequestModel.OrderId,existingModel, out _notFoundObjectResult))
            //{
            //    return _notFoundObjectResult;
            //}
            
            //var existingModelETag = CreateMD5(existingModel.ModifiedDate.ToString());
            //if (Request != null)
            //{
            //    if (Request.Headers.ContainsKey("If-Match") && Request.Headers["If-Match"] != existingModelETag)
            //    {
            //        return StatusCode(412, "The data on this page is out of date, please refresh the page.");
            //    }
            //}

            //var jwtToken = GetToken();
            ////CreateVRO is true for i360 ISA triggered Create VRO flow from scheduler  
            //if (repairRequestModel.CreateVRO && Convert.ToInt32(existingModel.OrderId) <= 0)
            //{ 
            //    var response = await _repairInvoiceService.SaveOnPremAsync(repairRequestModel.RepairInvoice, jwtToken);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var responseDataDeserialized = (JObject)JsonConvert.DeserializeObject(await response.Content?.ReadAsStringAsync());
            //        _logger.LogInformation($"Invoke SaveOnPremAsync - Success {responseDataDeserialized}");
            //        responseDataDeserialized.TryGetValue("orderId", out JToken orderId);
            //        responseDataDeserialized.TryGetValue("objectId", out JToken objectId);
            //        responseDataDeserialized.TryGetValue("objectType", out JToken objectType);
            //        existingModel.CreateVRO = false;
            //        existingModel.OrderId = Convert.ToString(orderId);
            //        repairRequestModel.Details.ObjId  = Convert.ToString(objectId);
            //        repairRequestModel.Details.ObjType = Convert.ToString(objectType);
            //    }
            //    else
            //    {
            //        _logger.LogInformation($"Invoke SaveOnPremAsync - Failure {await response.Content?.ReadAsStringAsync()}");
            //    }
            //}

            //if (repairRequestModel.Details.RequestStatus.ToUpper() == "DELIVERED")
            //{
            //    await _auditService.PostAuditAsync(repairRequestModel, jwtToken);
            //}

            ////Invoke repair invoice service to update
            //await _repairRequestService.ReplaceOneAsync(existingModel, repairRequestModel);
            return NoContent();
        }

        //public static string CreateMD5(string input)
        //{
        //    // Use input string to calculate MD5 hash
        //    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        //    {
        //        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        //        byte[] hashBytes = md5.ComputeHash(inputBytes);

        //        return Convert.ToHexString(hashBytes);
        //    }
        //}

        //private JwtInfo GetToken()
        //{
        //    if (HttpContext == null)
        //        return null;

        //    var jwtinfo = (JwtInfo)HttpContext.Items[AppConstants.JwtInfo];
        //    return jwtinfo;
        //}

      

        
    }
}
