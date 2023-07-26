using repair.service.data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.unit.tests.data
{
    public class RepairInvoiceMockData
    {
        public RepairInvoice MockRepairInvoiceCreate()
        {
            return MockRepairInvoice();
        }
        public RepairInvoice MockRepairInvoice()
        {
            return new RepairInvoice
            {
                Id = "1",
                AccountId = "123",
                CreatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = "admin",
                ModifiedDate = DateTime.UtcNow,
                OrderId = 125,
                OrderNumber = "",
                PurchaseOrderNumber = "DVPO-0002815",
                RepairShop = "01",
                Status = "OPEN",
                OpenedDate = DateTime.UtcNow,
                Priority = 4,
                AmountUnitOfMeasure = "US$",
                IsWarranty = true,
                ClosedBy = "",
                UnitId = "1",
                UnitNumber = "2002",
                CustomerId = 0,
                CustomerName = "",
                CompanyId = "TMT",
                CompletedBy = "abc",
                RepairClass = "SCHEDULED",
                RepairSite = "CUSTOMER",
                VendorEstimate = 350,
                CustomerEstimate = 22,
                CostCenter = "1-2-01-1",
                PriceTableId = "1",
                PriceTableDescription = "MARKUP 50%",
                Vendor = "NAPA1",
                PoInvoiceNumber = "PO-A12345",
                PoInvoiceDate = DateTime.UtcNow,
                PoInvoiceAmount = "5.00",
                MeterType = "ODOMETER",
                MeterReading = 230500,
                Sections = MockRepairInvoiceSection()

            };
        }
        private static List<RepairInvoiceSection> MockRepairInvoiceSection()
        {
            return new List<RepairInvoiceSection>()
            {
                new RepairInvoiceSection()
                {
                    SectionId = 0,
                    SectionNumber= 0,
                    SectionStatus= "OPEN",
                    IsBillable= true,
                    OpenedDate= DateTime.UtcNow,
                    RepairReason= "ACTOFGOD",
                    ComponentCode= "001-002-001",
                    Complaint= "Broken",
                    Priority= 5,
                    DelayReason= null,
                    ModifiedDate= DateTime.UtcNow,
                    ModifiedBy= "admin",
                    SectionComment= "COMPLAINT: is not working",
                    WarrantyType= null,
                    Lines = MockRepairInvoiceLines()
                }

            };

        }
        private static List<Line> MockRepairInvoiceLines()
        {
            return new List<Line>()
            {
              new Line()
              {
                LineId="12",
                LineNumber="545",
                LineType="PART",
                LineStatus="COMPLETE",
                ComponentCode="002-010-055",
                Description="MMI 3 2310 MOTORZHEATDW/SWI",
                MechanicId="",
                MechanicName="",
                IsVendorLine=true,
                QuantityOrHours=0,
                QuantityRequested=6,
                QuantityRecieved="0",
                QuantityUnitOfMeasure="FOOT",
                ChargeAmount=250.0,
                ChargeUnitOfMeasure="CN$",
                ChargeCategory="STANDARD",
                TaxAmount="0",
                ChargeDate=DateTime.UtcNow,
                ModifiedDate=DateTime.UtcNow,
                ModifiedBy="ta_interface",
                AccountType="",
                PayGrade="",
                Variance="0",
                LineTotal=1500,
                AfterMarket="",
                PartNumber="SAMP-125",
                ItemId="",
                ManufacturerCode="IMPER",
                LineItems = new List<LineItem> ()

              }   
            };

        }

        public IEnumerable<RepairInvoice> GetExistingMockRepairInvoice()
        {
            return new List<RepairInvoice>()
            {
                new RepairInvoice()
               {
                   Id = "1",
                   AccountId = "123",
                   ModifiedDate = DateTime.Now,
                   ModifiedBy = "admin",
                   OrderId = 125
               }
            };

        }

        public RepairInvoice GetMockRepairInvoiceForToBeUpdate()
        {
            return new RepairInvoice()
            {
                AccountId = "73e521bf72add61c48841084",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "admin",
                Id = "1"
            };

        }
    }
}
