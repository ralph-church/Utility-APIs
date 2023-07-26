using repair.service.repository.abstracts;

namespace repair.service.repository.config
{
    public class TtcServicesConfig : ITtcServicesConfig
    {
        public string RepairInvoiceEndPoint { get; set; }
        public string AuditEndPoint { get; set; }
    }
}
