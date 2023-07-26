using repair.service.service.abstracts;
using System;

namespace repair.service.service.model
{
    public class OutgoingMessage : IOutgoingMessage
    {        
        public string Data { get; set; }
        public string EventType { get; set; }
        public Uri EventSource { get; set; }
        public string EventSubject { get; set; }
        public string EventId { get; set; }
    }
}
