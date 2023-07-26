namespace repair.service.service.model
{
    public class ErrorAuditModel : TDocumentModel
    {
        public string EventId { get; set; }
        public string EventType { get; set; }
        public string EventSource { get; set; }
        public string EventSubject { get; set; }
        public string Payload { get; set; }
        public string RoutingGatewayRegistryId { get; set; }
        public string ErrorState { get; set; }
        public string ErrorMessage { get; set; }
        public int RetryAttempt { get; set; }
    }

}