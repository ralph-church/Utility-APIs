using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;

namespace repair.service.data.entities
{
    [BsonCollection("repair_invoice_stages")]
    public class RepairInvoiceStage : RepairInvoice
    {
        [BsonElement("retry_count")]
        public int RetryCount { get; set; }
        [BsonElement("event_id")]
        public string EventId { get; set; }
        [BsonElement("message_id")]
        public string MessageId { get; set; }
        [BsonElement("event_type")]
        public string EventType { get; set; }
  
    }
}
