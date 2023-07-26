using MongoDB.Bson.Serialization.Attributes;
using repair.service.data.utilities;
using System;
using System.Collections.Generic;

namespace repair.service.data.entities
{
    [BsonCollection("repair_invoices")]
    public class RepairInvoice : TDocument
    {

        [BsonElement("integration_instance_id")]
        public string IntegrationInstanceId { get; set; }

        [BsonElement("integration_type")]
        public string IntegrationType { get; set; }

        [BsonElement("account_id")]
        public string AccountId { get; set; }

        [BsonElement("integration_name")]
        public string IntegrationName { get; set; }
        [BsonElement("order_id")]
        public int OrderId { get; set; }
        [BsonElement("order_number")]
        public string OrderNumber { get; set; }
        [BsonElement("purchase_order_number")]
        public string PurchaseOrderNumber { get; set; }
        [BsonElement("repair_shop")]
        public string RepairShop { get; set; }
        [BsonElement("status")]
        public string Status { get; set; }
        [BsonElement("Opened_date")]
        public DateTime OpenedDate { get; set; }
        [BsonElement("closed_date")]
        public DateTime? ClosedDate { get; set; }
        [BsonElement("priority")]
        public int Priority { get; set; }
        [BsonElement("amount_unit_of_measure")]
        public string AmountUnitOfMeasure { get; set; }
        [BsonElement("is_warranty")]
        public bool IsWarranty { get; set; }
        [BsonElement("closed_by")]
        public string ClosedBy { get; set; }
        [BsonElement("closed_on")]
        public DateTime? ClosedOn { get; set; }
        [BsonElement("completed_date")]
        public DateTime? CompletedDate { get; set; }
        [BsonElement("unit_id")]
        public string UnitId { get; set; }
        [BsonElement("unit_number")]
        public string UnitNumber { get; set; }
        [BsonElement("unit_customer_name")]
        public string UnitCustomerName { get; set; }
        [BsonElement("vin_number")]
        public string VINNumber { get; set; }
        [BsonElement("customer_id")]
        public int CustomerId { get; set; }
        [BsonElement("customer_name")]
        public string CustomerName { get; set; }
        [BsonElement("company_id")]
        public string CompanyId { get; set; }
        [BsonElement("completed_by")]
        public string CompletedBy { get; set; }
        [BsonElement("repair_class")]
        public string RepairClass { get; set; }
        [BsonElement("repair_site")]
        public string RepairSite { get; set; }
        [BsonElement("repair_type")]
        public string RepairType { get; set; }
        [BsonElement("vendor_estimate")]
        public double VendorEstimate { get; set; }
        [BsonElement("customer_estimate")]
        public double CustomerEstimate { get; set; }
        [BsonElement("promised_by_date")]
        public DateTime? PromisedByDate { get; set; }
        [BsonElement("cost_center")]
        public string CostCenter { get; set; }
        [BsonElement("price_table_id")]
        public string PriceTableId { get; set; }
        [BsonElement("price_table_description")]
        public string PriceTableDescription { get; set; }
        [BsonElement("vendor")]
        public string Vendor { get; set; }
        [BsonElement("po_invoice_number")]
        public string PoInvoiceNumber { get; set; }
        [BsonElement("po_invoice_date")]
        public DateTime? PoInvoiceDate { get; set; }
        [BsonElement("po_invoice_amount")]
        public string PoInvoiceAmount { get; set; }
        [BsonElement("meter_type")]
        public string MeterType { get; set; }
        [BsonElement("meter_reading")]
        public int MeterReading { get; set; }
        [BsonElement("validate_po_approval")]
        public bool ValidatePOApproval { get; set; }
        [BsonElement("customer_po")]
        public string CustomerPO { get; set; }
        [BsonElement("section")]
        public List<RepairInvoiceSection> Sections { get; set; }      
    }
    [BsonCollection("sections")]
    public class RepairInvoiceSection
    {
        [BsonElement("section_id")]
        public int SectionId { get; set; }
        [BsonElement("section_number")]
        public int SectionNumber { get; set; }
        [BsonElement("section_status")]
        public string SectionStatus { get; set; }
        [BsonElement("is_billable")]
        public bool IsBillable { get; set; }
        [BsonElement("opened_date")]
        public DateTime OpenedDate { get; set; }
        [BsonElement("completed_date")]
        public DateTime? CompletedDate { get; set; }
        [BsonElement("repair_reason")]
        public string RepairReason { get; set; }
        [BsonElement("component_code")]
        public string ComponentCode { get; set; }
        [BsonElement("component_codekey")]
        public string ComponentCodeKey { get; set; }
        [BsonElement("complaint")]
        public string Complaint { get; set; }
        [BsonElement("priority")]
        public int Priority { get; set; }
        [BsonElement("delay_reason")]
        public string DelayReason { get; set; }
        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; }
        [BsonElement("modified_by")]
        public string ModifiedBy { get; set; }
        [BsonElement("section_comment")]
        public string SectionComment { get; set; }
        [BsonElement("warranty_type")]
        public string WarrantyType { get; set; }
        [BsonElement("lines")]
        public List<Line> Lines { get; set; }
    }
    [BsonCollection("lines")]
    public class Line
    {
        [BsonElement("line_id")]
        public string LineId { get; set; }
        [BsonElement("line_number")]
        public string LineNumber { get; set; }
        [BsonElement("line_type")]
        public string LineType { get; set; }
        [BsonElement("line_status")]
        public string LineStatus { get; set; }
        [BsonElement("component_code")]
        public string ComponentCode { get; set; }
        [BsonElement("component_codekey")]
        public string ComponentCodeKey { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("mechanic_id")]
        public string MechanicId { get; set; }
        [BsonElement("mechanic_name")]
        public string MechanicName { get; set; }
        [BsonElement("is_vendor_line")]
        public bool IsVendorLine { get; set; }
        [BsonElement("quantity_or_hours")]
        public int QuantityOrHours { get; set; }
        [BsonElement("quantity_recieved")]
        public string QuantityRecieved { get; set; }
        [BsonElement("quantity_requested")]
        public int QuantityRequested { get; set; }
        [BsonElement("quantity_unit_of_measure")]
        public string QuantityUnitOfMeasure { get; set; }
        [BsonElement("charge_amount")]
        public double ChargeAmount { get; set; }
        [BsonElement("charge_unit_of_measure")]
        public string ChargeUnitOfMeasure { get; set; }
        [BsonElement("charge_category")]
        public string ChargeCategory { get; set; }
        [BsonElement("tax_amount")]
        public string TaxAmount { get; set; }
        [BsonElement("charge_date")]
        public DateTime? ChargeDate { get; set; }
        [BsonElement("account_type")]
        public string AccountType { get; set; }
        [BsonElement("no_charge")]
        public string NoCharge { get; set; }
        [BsonElement("pay_grade")]
        public string PayGrade { get; set; }
        [BsonElement("variance")]
        public string Variance { get; set; }
        [BsonElement("line_total")]
        public int LineTotal { get; set; }
        [BsonElement("after_market")]
        public string AfterMarket { get; set; }
        [BsonElement("part_number")]
        public string PartNumber { get; set; }
        [BsonElement("item_id")]
        public string ItemId { get; set; }
        [BsonElement("manufacturer_code")]
        public string ManufacturerCode { get; set; }
        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; }
        [BsonElement("modified_by")]
        public string ModifiedBy { get; set; }
        [BsonElement("line_items")]
        public List<LineItem> LineItems { get; set; }
    }
    [BsonCollection("line_items")]
    public class LineItem
    {
        [BsonElement("code_type")]
        public string CodeType { get; set; }
        [BsonElement("code")]
        public string Code { get; set; }
    }
}
