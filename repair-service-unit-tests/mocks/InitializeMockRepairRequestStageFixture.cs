using AutoMapper;
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
    public class InitializeMockRepairRequestStageFixture
    {
        public Mock<IDataRepository<RepairRequestStage>> MockDataRepository { get; }
        public Mock<IDataRepository<RepairRequest>> MockRepairRequestDataRepository { get; }

        public Mock<RepairRequestStageMockData> MockData { get; }
        public IMapper Mapper { get; }
        public Mock<IDatabaseSettings> MockDatabaseSettings { get; }
        public Mock<RepairRequestStageService> MockRepairStageRequestService { get; }
        public Mock<RepairRequestService> MockRepairRequestService { get; }
      

        public InitializeMockRepairRequestStageFixture()
        {
            Mapper = AutoMapperConfiguration.Configure();
            MockDataRepository = new Mock<IDataRepository<RepairRequestStage>>();
            MockRepairRequestDataRepository = new Mock<IDataRepository<RepairRequest>>();
            MockData = new Mock<RepairRequestStageMockData>();
            MockDatabaseSettings = new Mock<IDatabaseSettings>();
            MockRepairStageRequestService = new Mock<RepairRequestStageService>(new NullLogger<RepairRequestStageService>(),
                                        MockDataRepository.Object, Mapper)
                                        { CallBase = true };
            MockRepairRequestService = new Mock<RepairRequestService>(                                     
                                      new NullLogger<RepairRequestService>(),
                                      MockRepairRequestDataRepository.Object, Mapper)
                                       { CallBase = true };
        }
    }
}
