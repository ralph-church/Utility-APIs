using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;


namespace repair.service.data.entities
{
    [BsonCollection("error_audit")]
    public class ErrorAudit : TDocument
    {
        [BsonElement("eventId")]
        public string EventId { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("eventSource")]
        public string EventSource { get; set; }

        [BsonElement("eventSubject")]
        public string EventSubject { get; set; }

        [BsonElement("payload")]
        public string Payload { get; set; }

        [BsonElement("routingGatewayRegistryId")]
        public string RoutingGatewayRegistryId { get; set; }

        [BsonElement("errorState")]
        public string ErrorState { get; set; }

        [BsonElement("errorMessage")]
        public string ErrorMessage { get; set; }

        [BsonElement("retryAttempt")]
        public int RetryAttempt { get; set; }
    
    }
}
