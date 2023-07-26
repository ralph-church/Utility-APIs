using repair.service.data.entities;
using repair.service.shared.paging;
using System;
using System.Collections.Generic;

namespace repair.service.unit.tests.data
{
    public class RepairInvoiceStageMockData
    {
        public RepairInvoiceStage MockRepairInvoiceStageCreate()
        {
            return MockRepairInvoiceStage();
        }
        public PagedResponseList<RepairInvoiceStage> MockRepairInvoiceStageGetAll()
        {
            return GetAllRepairInvoice();
        }
        public PagedResponseList<RepairInvoiceStage> GetAllRepairInvoice()
        {
            var repairInvoiceStageList = new List<RepairInvoiceStage>();
            repairInvoiceStageList.Add(MockRepairInvoiceStage());
            return new PagedResponseList<RepairInvoiceStage>()
            {
                Data = repairInvoiceStageList,
                Paging = new PagingInfo<RepairInvoiceStage>() { PageNo = 1, PageCount = 1, PageSize = 1,TotalRecordCount=1 }
            };
        }
        public RepairInvoiceStage MockRepairInvoiceStage()
        {
            return new RepairInvoiceStage
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
                Sections = MockRepairInvoiceSection(),
                RetryCount = 1,
                EventId= "ca967a13-b51a-4a02-960e-4e57a72fa152",
                MessageId="123"

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
        public IEnumerable<RepairInvoiceStage> GetExistingMockRepairInvoiceStage()
        {
            return new List<RepairInvoiceStage>()
            {
                new RepairInvoiceStage()
               {
                   Id = "1",
                   AccountId = "123",
                   ModifiedDate = DateTime.Now,
                   ModifiedBy = "admin",
                   OrderId = 125
               }
            };

        }
        public RepairInvoiceStage GetMockRepairInvoiceStageForToBeUpdate()
        {
            return new RepairInvoiceStage()
            {
                AccountId = "73e521bf72add61c48841084",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "admin",
                Id = "1"
            };

        }
    }
}
