using System;

namespace repair.service.service.abstracts
{
    public interface IDocumentModel
    {
        public string Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedBy { get; set; }
    }
}
