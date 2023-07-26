using System;

namespace repair.service.service.model 
{
    public class NotificationModel : TDocumentModel
    {

        public string LocationWorkOrder { get; set; }

        public string Tenant { get; set; }

        public string IntegrationName { get; set; }

        public TANotificationModel Data { get; set; }

        public bool IsProcessed { get; set; }

        public DateTime ProcessedOn { get; set; }

        public int RetryCount { get; set; }

        public Nullable<int> ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

    }

    public class TANotificationModel
    {
        public string CusNo { get; set; }

        public string EventTs { get; set; }

        public string LocationCd { get; set; }

        public string WONbr { get; set; }

        public string WoSsStepName { get; set; }

        public string WoSsStepStatusName { get; set; }
    }
}
