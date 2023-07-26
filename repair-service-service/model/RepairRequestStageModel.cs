namespace repair.service.service.model
{

    public class RepairRequestStageModel : TDocumentModel
    {        
        public string AccountId { get; set; }
        public string EventId { get; set; }
        public string EventType { get; set; }
        public string CallerActionType { get; set; }
        public string ReferenceNumber { get; set; }
        public string InstanceId { get; set; }
        public object ResponseData { get; set; }
    }
}
