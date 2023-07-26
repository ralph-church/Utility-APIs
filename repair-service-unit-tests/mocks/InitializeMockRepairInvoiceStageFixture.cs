using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using repair.service.data.entities;
using repair.service.repository.abstracts;
using repair.service.service;
using repair.service.service.mapping;
using repair.service.shared.abstracts;
using repair.service.unit.tests.data;

namespace repair.service.unit.tests.mocks
{
    public class InitializeMockRepairInvoiceStageFixture
    {
        public Mock<RepairInvoiceStageService> MockRepairInvoiceStageService { get; }
        public Mock<IDataRepository<RepairInvoiceStage>> MockDataRepository { get; }
        public Mock<RepairInvoiceStageMockData> MockRepairInvoiceStageData { get; }
        public Mock<ILogger<RepairInvoiceStageService>> MockLogger { get; }
        public IMapper Mapper { get; }

        public Mock<IHttpService> mockHttpRequest { get; }

        public Mock<ITtcServicesConfig> mockTtcServicesConfig { get; }

        public InitializeMockRepairInvoiceStageFixture()
        {
            MockRepairInvoiceStageData = new Mock<RepairInvoiceStageMockData>();
            Mapper = AutoMapperConfiguration.Configure();
            MockDataRepository = new Mock<IDataRepository<RepairInvoiceStage>>();
            MockLogger = new Mock<ILogger<RepairInvoiceStageService>>();
            mockHttpRequest = new Mock<IHttpService>();
            MockRepairInvoiceStageService = new Mock<RepairInvoiceStageService>(new NullLogger<RepairInvoiceStageService>(),
                                        MockDataRepository.Object, Mapper, mockHttpRequest.Object)
            { CallBase = true };
        }
    }
}
