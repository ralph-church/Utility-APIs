using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;

namespace repair.service.data.entities
{
    [BsonCollection("repair_request_stages")]
    public class RepairRequestStage : TDocument
    {    

        [BsonElement("account_id")]
        public string AccountId { get; set; }
        [BsonElement("event_id")]
        public string EventId { get; set; }
        [BsonElement("event_type")]
        public string EventType { get; set; }
        [BsonElement("reference_number")]
        public string ReferenceNumber { get; set; }       
        [BsonElement("configuration_instance_id")]
        public string InstanceId { get; set; }
        [BsonElement("caller_action_type")]
        public string CallerActionType { get; set; }

        [BsonElement("response_data")]
        public BsonDocument ResponseData { get; set; }

    }
}
