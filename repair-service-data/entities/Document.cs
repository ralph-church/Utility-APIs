using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.abstracts;
using System;

namespace repair.service.data.entities
{
    public class TDocument : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }
        [BsonElement("created_by")]
        public string CreatedBy { get; set; }
        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; }
        [BsonElement("modified_by")]
        public string ModifiedBy { get; set; }

    }
}
