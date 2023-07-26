using repair.service.shared.exception.model;
using System;
using System.Net;

namespace repair.service.shared.exception.infrastructure
{
    public class ExceptionFactory
    {
        public static ErrorModel GetErrorModel(System.Exception exception, ExceptionOption option)
        {
           
            HttpStatusCode status ;
            string message;
            switch (exception)
            {
                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    message = ExceptionConstant.ERROR_UNAUTHORIZED;
                    break;
                case NotImplementedException:
                    status = HttpStatusCode.NotImplemented;
                    message = ExceptionConstant.ERROR_NOTIMPLEMENTED;
                    break;
                case TimeoutException:
                    status = HttpStatusCode.RequestTimeout;                
                    message = exception.Message; //ExceptionConstant.ERROR_TIMEOUT;
                    break;
                case FormatException:
                    status = HttpStatusCode.UnprocessableEntity;
                    message = string.Format(ExceptionConstant.ERROR_FORMAT, exception.Message);
                    break;
                case InvalidOperationException:
                    status = HttpStatusCode.UnprocessableEntity;
                    message = exception.Message;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = string.Format(ExceptionConstant.ERROR_INTERNALSERVER, exception.Message);
                    break;
            }

            var errorModel = new ErrorModel() { StatusCode = (int)status, Message = message };

            if (option.EnableTrace)
                errorModel.Details = exception.StackTrace;

            return errorModel;
        }

    }
}
