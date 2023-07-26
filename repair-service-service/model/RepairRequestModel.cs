using System;

namespace repair.service.service.model
{
    public class RepairRequestModel : TDocumentModel
    {

        public string AccountId { get; set; }

        public string IntegrationName { get; set; }

        public string OrderId { get; set; }        

        public DetailModel Details { get; set; }

        public bool SendToExternalSystem { get; set; }

        public bool IsSentToExternalSystem { get; set; }

        public bool CreateVRO { get; set; }

        public RepairInvoiceModel RepairInvoice { get; set; }

        public string IntegrationType { get; set; }

    }

    public class DetailModel
    {
        public string RequestNumber { get; set; }

        public string RequestStatus { get; set; }

        public string Vin { get; set; }

        public string Vendor { get; set; }

        public string Customer { get; set; }

        public string Shop { get; set; }

        public Nullable<DateTime> ETA { get; set; }

        public Nullable<DateTime> ROETC { get; set; } // RO Estimated time of completion

        public CommentModel[] Comments { get; set; }

        public SectionModel[] Sections { get; set; }

        public string InstanceId { get; set; }

        public string ObjId { get; set; }

        public string ObjType { get; set; }
        public int CustomerId { get; set; }
        public string IsActionRequired { get; set; }
    }

    public class CommentModel
    {
        public string CommentId { get; set; }

        public string CommentText { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Source { get; set; }

        public string Status { get; set; }

        public bool IsSubmitted { get; set; }

        public string ParentId { get; set; }

        public string UserCompanyName { get; set; }
        public string IndividualUserID { get; set; }
    }

    public class SectionModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public string Source { get; set; }

        public bool IsSubmitted { get; set; }
    }
}
