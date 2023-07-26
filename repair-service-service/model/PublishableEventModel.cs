using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.service.model
{
    public class PublishableEventModel:TDocumentModel
    {
        public string EventType { get; set; }
        public string Host { get; set; }
        public string EntityName { get; set; }
        public string InboundSubscription { get; set; }
        public string Key { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public int? MessageMaxBytes { get; set; }
    }
}
