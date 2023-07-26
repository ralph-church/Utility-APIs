using repair.service.data.entities;
using repair.service.service.model;
using repair.service.shared.paging;
using System;
using System.Collections.Generic;

namespace repair.service.unit.tests.data
{
    public class RepairRequestMockData
    {

        public string accountId = "234db243-8dfb-49e4-89ab-3d284f550113";

        public static IEnumerable<RepairRequestModel> GetMockRepairRequestModels()
        {
            return new List<RepairRequestModel>()
            {
                new RepairRequestModel()
                {
                    Id = "1"
                },
                new RepairRequestModel()
                {
                    Id = "2"
                }
            };
        }

        public PagedResponseList<RepairRequest> GetMockRepairRequestsWithPaging()
        {
            var result = PagedResponseList<RepairRequest>.ToPagedResponseList(null, 0, 0);
            result.Data = new List<RepairRequest>();

            result.Data.Add(MockRepairRequest("1"));
            result.Data.Add(MockRepairRequest("2"));
            return result;
        }

        public RepairRequestModel GetMockRepairRequestModel()
        {
            return new RepairRequestModel()
            {
                Id = "1",
                AccountId = "234db243-8dfb-49e4-89ab-3d284f550113",
                IntegrationName = "534db243-8dfb-49e4-89ab-3d284f550113",
                OrderId = "6554"
            };
        }

        #region MockData For Create
        public RepairRequest GetMockRepairRequestForToBeCreate()
        {
            return MockRepairRequest("0");
        }

        public RepairRequest GetMockRepairRequestForCreated()
        {
            RepairRequest template = MockRepairRequest("2");
            return template;
        }


        #endregion

        #region MockData For Update

        public RepairRequest GetMockRepairRequestForToBeUpdate()
        {
            RepairRequest template = MockRepairRequest("1");
            return template;

        }


        public IEnumerable<RepairRequest> GetExistingMockRepairRequests()
        {
            return GetMockRepairRequests();
        }

        #endregion

        public IEnumerable<RepairRequest> GetMockRepairRequests()
        {
            List<RepairRequest> repairRequests = new List<RepairRequest>();
            repairRequests.Add(MockRepairRequest("1"));
            repairRequests.Add(MockRepairRequest("2"));
            return repairRequests;
        }

        public RepairRequest MockRepairRequest(string id)
        {
            return new RepairRequest
            {
                Id = id,
                CreatedBy = "admin",
                //CreatedDate =  DateTime.UtcNow,
                ModifiedBy = "admin",
                //ModifiedDate = DateTime.UtcNow,                
                OrderId = "6554",
                AccountId = "234db243-8dfb-49e4-89ab-3d284f550113",
                IntegrationName = "534db243-8dfb-49e4-89ab-3d284f550113",
                Details = MockRepairRequestDetails(),
                SendToExternalSystem = true,                
                IsSentToExternalSystem = false,                
            };
        }

        private static Detail MockRepairRequestDetails()
        {
            Detail details = new Detail()
            {
                RequestNumber = "21981",
                RequestStatus = "Service Request",
                Vin = "219",
                Vendor = "624957000",
                Shop = "A7",
                Customer = "C53",
            };
            details.Comments = new Comment[] { MockRepairRequestDetailsComments() };
            details.Sections = new Section[] { MockRepairRequestDetailsSections() };
            return details;
        }

        private static Comment MockRepairRequestDetailsComments()
        {
            Comment comments = new Comment()
            {
                CommentId = "28845",
                CommentText = "A Service Request has been created for this vehicle",
                CreatedBy = "Trimble User",
                CreatedDate = DateTime.Now,
                Source = "S654",
                Status = "S",
                IsSubmitted = true,
                ParentId = ""
            };            
            return comments;
        }

        private static Section MockRepairRequestDetailsSections()
        {
            Section sections = new Section()
            {
                Title = "Backing Plate - Front Brake",
                Description = "Backing Plate - Front Brake",
                Comments = "Backing Plate - Front Brake",
                Source = "S456",
                IsSubmitted = true
            };
            return sections;
        }
    }
}
