using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service;
using repair.service.service.mapping;
using repair.service.shared.abstracts;
using repair.service.shared.paging;
using repair.service.unit.tests.data;
using System.Collections.Generic;

namespace repair.service.unit.tests.mocks
{
    public class InitializeMockRepairRequestFixture
    {
        public Mock<IHttpService> mockHttpRequest { get; }

        public Mock<RepairRequestService> MockRepairRequestService { get; }

        public Mock<RepairInvoiceService> MockRepairInvoiceService { get; }
        public Mock<IDataRepository<RepairRequest>> MockDataRepository { get; }
        public Mock<IDataRepository<RepairInvoice>> MockDataRepositoryRepairInvoice { get; }
        public Mock<RepairRequestMockData> MockData { get; }
        public IMapper Mapper { get; }
        public Mock<QueryStringParams> MockQueryStringParams { get; }
        public Mock<ITtcServicesConfig> mockTtcServicesConfig { get; }

        public Mock<AuditService> MockAuditService { get; }

        public InitializeMockRepairRequestFixture()
        {
            MockData = new Mock<RepairRequestMockData>();
            Mapper = AutoMapperConfiguration.Configure();
            mockHttpRequest = new Mock<IHttpService>();
            MockDataRepository = new Mock<IDataRepository<RepairRequest>>();
            MockDataRepositoryRepairInvoice = new Mock<IDataRepository<RepairInvoice>>();
            mockTtcServicesConfig = new Mock<ITtcServicesConfig>();
            MockRepairRequestService = new Mock<RepairRequestService>(
                                        new NullLogger<RepairRequestService>(),
                                        MockDataRepository.Object, Mapper)
                                        { CallBase = true };
            MockQueryStringParams = new Mock<QueryStringParams>();
            MockQueryStringParams.Setup(x => x.PageNo).Returns(1);
            MockQueryStringParams.Setup(x => x.PageSize).Returns(10);
            MockQueryStringParams.Setup(x => x.SortBy).Returns(new List<Sorting>());

            MockRepairInvoiceService = new Mock<RepairInvoiceService>(new NullLogger<RepairInvoiceService>(),
                                       MockDataRepositoryRepairInvoice.Object, Mapper, mockHttpRequest.Object, mockTtcServicesConfig.Object)
            { CallBase = true };

            MockAuditService = new Mock<AuditService>(
                new NullLogger<AuditService>(), 
                mockHttpRequest.Object, 
                mockTtcServicesConfig.Object,
                null
                );

         
        }
    }
}
