using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;
using System;

namespace repair.service.data.entities
{
    [BsonCollection("repair_requests")]
    public class RepairRequest : TDocument
    {
        [BsonElement("account_id")]
        public string AccountId { get; set; }

        [BsonElement("integration_name")]
        public string IntegrationName { get; set; }

        [BsonElement("order_id")]
        public string OrderId { get; set; }       
        
        [BsonElement("details")]
        public Detail Details { get; set; }

        [BsonElement("send_to_external_system")]
        public bool SendToExternalSystem { get; set; }

        [BsonElement("is_sent_to_external_system")]
        public bool IsSentToExternalSystem { get; set; }

        [BsonElement("create_vro")]
        public bool CreateVRO { get; set; } = false;

    }

    [BsonCollection("details")]
    public class Detail
    {
        [BsonElement("request_number")]
        public string RequestNumber { get; set; }

        [BsonElement("request_status")]
        public string RequestStatus { get; set; }

        [BsonElement("vin")]
        public string Vin { get; set; }

        [BsonElement("vendor")]
        public string Vendor { get; set; }

        [BsonElement("customer")]
        public string Customer { get; set; }

        [BsonElement("shop")]
        public string Shop { get; set; }

        [BsonElement("eta")]
        public Nullable<DateTime> ETA { get; set; }

        [BsonElement("roetc")]
        public Nullable<DateTime> ROETC { get; set; }

        [BsonElement("comments")]
        public Comment[] Comments { get; set; }

        [BsonElement("sections")]
        public Section[] Sections { get; set; }

        [BsonElement("obj_id")]
        public string ObjId { get; set; }

        [BsonElement("obj_type")]
        public string ObjType { get; set; }

        [BsonElement("instance_id")]
        public string InstanceId { get; set; }

        [BsonElement("customer_id")]
        public int CustomerId { get; set; }
        [BsonElement("is_action_required")]
        public string IsActionRequired { get; set; }

    }

    [BsonCollection("comments")]
    public class Comment
    {
        [BsonElement("comment_id")]
        public string CommentId { get; set; }

        [BsonElement("comment_text")]
        public string CommentText { get; set; }

        [BsonElement("created_by")]
        public string CreatedBy { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("source")]
        public string Source { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("is_submitted")]
        public bool IsSubmitted { get; set; }

        [BsonElement("parent_id")]
        public string ParentId { get; set; }

        [BsonElement("user_company_name")]
        public string UserCompanyName { get; set; }
        [BsonElement("individual_user_id")]
        public string IndividualUserID { get; set; }
    }

    [BsonCollection("sections")]
    public class Section
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("source")]
        public string Source { get; set; }

        [BsonElement("is_submitted")]
        public bool IsSubmitted { get; set; }

    }
}







