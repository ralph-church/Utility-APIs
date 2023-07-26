using System;
using System.Collections.Generic;

namespace repair.service.service.model
{
    public class RepairInvoiceModel : TDocumentModel
    {
        public string IntegrationInstanceId { get; set; }
        public string IntegrationType { get; set; }
        public string IntegrationName { get; set; }
        public string AccountId { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string RepairShop { get; set; }
        public string Status { get; set; }
        public DateTime? OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? Priority { get; set; }
        public string AmountUnitOfMeasure { get; set; }
        public bool? IsWarranty { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int UnitId { get; set; }
        public string UnitNumber { get; set; }
        public string UnitCustomerName { get; set; }
        public string VINNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CompanyId { get; set; }
        public string CompletedBy { get; set; }
        public string RepairClass { get; set; }
        public string RepairSite { get; set; }
        public double VendorEstimate { get; set; }
        public double CustomerEstimate { get; set; }
        public DateTime? PromisedByDate { get; set; }
        public string CostCenter { get; set; }
        public string PriceTableId { get; set; }
        public string PriceTableDescription { get; set; }
        public string Vendor { get; set; }
        public string InvoiceNumber { get; set; }
        public string PoInvoiceNumber { get; set; }
        public DateTime? PoInvoiceDate { get; set; }
        public decimal PoInvoiceAmount { get; set; }
        public string MeterType { get; set; }
        public double? MeterReading { get; set; }
        public bool ValidatePOApproval { get; set; }
        public string CustomerPO { get; set; }
        public List<RepairInvoiceSectionModel> Sections { get; set; }
        public string PayMethod { get; set; }
        public string RepairType { get; set; }
        public string WarrantyType { get; set; }
        public int? ObjectId { get; set; }
        public int? ObjectType { get; set; }


    }
    public class RepairInvoiceSectionModel
    {
        public int SectionId { get; set; }
        public int SectionNumber { get; set; }
        public string SectionStatus { get; set; }
        public bool IsBillable { get; set; }
        public DateTime? OpenedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string RepairReason { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentCodeKey { get; set; }
        public string Complaint { get; set; }
        public int Priority { get; set; }
        public string DelayReason { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string SectionComment { get; set; }
        public string WarrantyType { get; set; }
        public List<LineModel> Lines { get; set; }
    }
    public class LineModel
    {
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public string LineType { get; set; }
        public string LineStatus { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentCodeKey { get; set; }
        public string Description { get; set; }
        public string MechanicId { get; set; }
        public string MechanicName { get; set; }
        public bool IsVendorLine { get; set; }
        public double QuantityOrHours { get; set; }
        public double QuantityRecieved { get; set; }
        public double QuantityRequested { get; set; }
        public string QuantityUnitOfMeasure { get; set; }
        public double ChargeAmount { get; set; }
        public string ChargeUnitOfMeasure { get; set; }
        public string ChargeCategory { get; set; }
        public double TaxAmount { get; set; }
        public DateTime? ChargeDate { get; set; }
        public string AccountType { get; set; }
        public string NoCharge { get; set; }
        public string PayGrade { get; set; }
        public double Variance { get; set; }
        public double LineTotal { get; set; }
        public string AfterMarket { get; set; }
        public string PartNumber { get; set; }
        public string ItemId { get; set; }
        public string ManufacturerCode { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public List<LineItemModel> LineItems { get; set; }
    }
    public class LineItemModel
    {
        public string CodeType { get; set; }
        public string Code { get; set; }
    }
}
