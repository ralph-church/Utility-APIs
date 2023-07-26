using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;
using System;

namespace repair.service.data.entities
{
    [BsonCollection("notifications")]
    public class Notification : TDocument
    {
        [BsonElement("location_work_order")]
        public string LocationWorkOrder { get; set; }

        [BsonElement("tenant")]
        public string Tenant { get; set; }

        [BsonElement("integration_name")]
        public string IntegrationName { get; set; }

        [BsonElement("data")]
        public TANotification Data { get; set; }

        [BsonElement("send_to_external_system")]
        public bool SendToExternalSystem { get; set; }

        [BsonElement("is_sent_to_external_system")]
        public bool IsSentToExternalSystem { get; set; }

        [BsonElement("create_vro")]
        public bool CreateVRO { get; set; } = false;

    }

    [BsonCollection("data")]
    public class TANotification
    {
        [BsonElement("cus_no")]
        public string CusNo { get; set; }

        [BsonElement("event_ts")]
        public string EventTs { get; set; }

        [BsonElement("location_cd")]
        public string LocationCd { get; set; }

        [BsonElement("wo_nbr")]
        public int WoNbr { get; set; }

        [BsonElement("wo_ss_step_nm")]
        public string WoSsStepNm { get; set; }

        [BsonElement("wo_ss_step_status_nm")]
        public string WoSsStepStatusNm { get; set; }

        [BsonElement("is_processed")]
        public bool IsProcessed { get; set; }

        [BsonElement("processed_on")]
        public Nullable<DateTime> ProcessedOn { get; set; }

        [BsonElement("retry_count")]
        public int RetryCount { get; set; }

        [BsonElement("error_code")]
        public Nullable<int> ErrorCode { get; set; }

        [BsonElement("error_message")]
        public string ErrorMessage { get; set; }

    }
}







