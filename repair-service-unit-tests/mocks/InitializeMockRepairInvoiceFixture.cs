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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.unit.tests.mocks
{
    public class InitializeMockRepairInvoiceFixture
    {
        public Mock<RepairInvoiceService> MockRepairInvoiceService { get; }
        public Mock<IDataRepository<RepairInvoice>> MockDataRepository { get; }
        public Mock<RepairInvoiceMockData> MockRepairInvoiceData { get; }
        public Mock<ILogger<RepairInvoiceService>> MockLogger { get; }
        public IMapper Mapper { get; }

        public Mock<IHttpService> mockHttpRequest { get; }

        public Mock<ITtcServicesConfig> mockTtcServicesConfig { get; }

        public InitializeMockRepairInvoiceFixture()
        {
            MockRepairInvoiceData = new Mock<RepairInvoiceMockData>();
            Mapper = AutoMapperConfiguration.Configure();
            MockDataRepository = new Mock<IDataRepository<RepairInvoice>>();
            MockLogger = new Mock<ILogger<RepairInvoiceService>>();
            mockHttpRequest = new Mock<IHttpService>();
            mockTtcServicesConfig = new Mock<ITtcServicesConfig>();
            MockRepairInvoiceService = new Mock<RepairInvoiceService>(new NullLogger<RepairInvoiceService>(),
                                        MockDataRepository.Object, Mapper, mockHttpRequest.Object,mockTtcServicesConfig.Object)
                                       { CallBase = true };
        }
    }
}
