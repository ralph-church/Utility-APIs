using MongoDB.Bson.Serialization.Attributes;
using System;

namespace repair.service.data.abstracts
{
    public interface IDocument
    {
        [BsonId]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedBy { get; set; }
    }
}
