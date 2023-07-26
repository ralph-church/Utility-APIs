using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.data.entities
{
    [BsonCollection("publishable_events")]
    public class PublishEvents:TDocument
    {
        [BsonElement("host")]
        public string Host { get; set; }


        [BsonElement("entityName")]
        public string EntityName { get; set; }

        [BsonElement("inboundSubscription")]
        public string InboundSubscription { get; set; }

        [BsonElement("key")]
        public string Key { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }
         [BsonElement("callerActionType")]
        public string CallerActionType { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("messageMaxBytes")]
        public int? MessageMaxBytes { get; set; }
    }
}
