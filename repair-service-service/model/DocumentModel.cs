using repair.service.service.abstracts;
using System;

namespace repair.service.service.model
{
    public class TDocumentModel : IDocumentModel
    {      
        public string Id { get; set; }        
        public DateTime? CreatedDate { get; set; }        
        public string CreatedBy { get; set; }        
        public DateTime? ModifiedDate { get; set; }        
        public string ModifiedBy { get; set; }
    }
}
