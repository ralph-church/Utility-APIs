using Microsoft.AspNetCore.Mvc;
using repair.service.service.model;
using repair.service.shared.exception.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace repair.service.api.controllers
{
    [ApiController]
    public class NotificationsServiceControllerBase : ControllerBase
    {
        public const string CheckPageNo = "PageNo";
        public const string CheckPageSize = "PageSize";
        public const string CheckPageNoWithObject = "queryStringParams.PageNo";
        public const string CheckPageSizeWithObject = "queryStringParams.PageSize";

        protected BadRequestObjectResult CreateBadRequestError(string errorMessage)
        {
            return BadRequest(new ErrorModel
            {
                StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                Message = errorMessage
            });
        }

        protected bool IsBadRequest(string id, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;
            if (string.IsNullOrEmpty(id))
            {
                badRequestObjectResult = BadRequest(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = "Missing parameters"
                });

                return true;
            }
            return false;
        }

        protected bool IsBadRequest(int orderId, string AccountId, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;
            if ((orderId == 0) && string.IsNullOrEmpty(AccountId))
            {
                badRequestObjectResult = BadRequest(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = "Missing parameters"
                });

                return true;
            }
            return false;
        }

        protected bool IsBadRequest(string id, string orderId, string instanceId, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;
            if (string.IsNullOrEmpty(id))
            {
                badRequestObjectResult = BadRequest(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = "Missing parameters"
                });

                return true;
            }
            return false;
        }

        protected bool IsBadRequestWithNullValues(string accountId, string orderId, string instanceId, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;
            if (string.IsNullOrEmpty(accountId) && string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(instanceId))
            {
                badRequestObjectResult = BadRequest(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = "Missing parameters"
                });

                return true;
            }
            return false;
        }

        protected bool IsBadRequest(EventPublishModel sourceModel, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;

            if (sourceModel == null)
            {
                badRequestObjectResult = CreateBadRequestError("All parameters are missing");
                return true;
            }

            bool isNull = sourceModel.GetType().GetProperties()
                            .Any(p => p.GetValue(sourceModel) == null);

            if (isNull)
            {
                badRequestObjectResult = CreateBadRequestError("Missing parameters");
                return true;
            }
            return false;
        }

        protected bool IsBadRequest(TDocumentModel entityModel, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;
            if (entityModel == null)
            {
                badRequestObjectResult = BadRequest(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = "Missing parameters"
                });

                return true;
            }
            return false;
        }

        protected bool IsBadRequest(RepairRequestModel repairRequestModel, out BadRequestObjectResult badRequestObjectResult)
        {
            badRequestObjectResult = null;

            if (repairRequestModel == null)
            {
                badRequestObjectResult = CreateBadRequestError("All parameters are missing.");
                return true;
            }

            if (repairRequestModel.OrderId == null)
            {
                badRequestObjectResult = CreateBadRequestError("ReferenceId should not be null.");
                return true;
            }
            return false;
        }

        protected bool IsEntityNotFound(string id, TDocumentModel entityModel, out NotFoundObjectResult notFoundObjectResult)
        {
            notFoundObjectResult = null;
            if (entityModel == null)
            {
                notFoundObjectResult = NotFound(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = $"Resource with this id {id} doesn't found."
                });
                return true;
            }
            return false;
        }

        protected bool IsEntityNotFound(string accountId, string instanceId, string orderId, TDocumentModel entityModel, out NotFoundObjectResult notFoundObjectResult)
        {
            notFoundObjectResult = null;
            if (entityModel == null)
            {
                notFoundObjectResult = NotFound(new ErrorModel
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = $"Resource with this accountid {accountId},instanceid {instanceId} ,orderid {orderId} doesn't found."
                });
                return true;
            }
            return false;
        }

        protected bool IsEntityNotFound(string id, IEnumerable<TDocumentModel> entityModels, out NotFoundObjectResult notFoundObjectResult)
        {
            notFoundObjectResult = null;
            return IsEntityNotFound(id, entityModels.FirstOrDefault(), out notFoundObjectResult);
        }

        protected bool IsValidPaging(ICollection<string> queryKeys)
        {
            if (queryKeys != null && (queryKeys.Contains(CheckPageNo) || queryKeys.Contains(CheckPageSize)
            || queryKeys.Contains(CheckPageNoWithObject) || queryKeys.Contains(CheckPageSizeWithObject)))
            {
                return true;
            }
            return false;
        }
    }
}
