using repair.service.data.entities;
using repair.service.service.model;
using System;
using System.Collections.Generic;

namespace repair.service.unit.tests.data
{
    public class RepairRequestStageMockData
    {
        #region MockData For GET
        public IEnumerable<RepairRequestStage> GetMockRepairRequestStage()
        {
            return new List<RepairRequestStage>()
            {
                {
                    new RepairRequestStage()
                    {
                        CreatedBy = "admin",
                        CreatedDate = DateTime.Now,
                        ModifiedBy = "admin",
                        ModifiedDate = DateTime.Now,
                        EventId = "22457a13-b51a-4a02-960e-4e57a72fa447",
                        EventType = "repair.invoice",
                        ReferenceNumber = "6554",
                        AccountId = "234db243-8dfb-49e4-89ab-3d284f550113",
                        InstanceId = "TMT-TEST",
                        CallerActionType = "GETREPAIRORDERDETAIL"
                    }
                    
                }
               
            };
        }
        #endregion
    }
}
