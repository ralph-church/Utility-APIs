namespace repair.service.service.model
{
    public class RepairInvoiceStageModel : RepairInvoiceModel
    {
        public int RetryCount { get; set; }
        public string EventId { get; set; }
        public string MessageId { get; set; }
        public string EventType { get; set; }
    }
}
