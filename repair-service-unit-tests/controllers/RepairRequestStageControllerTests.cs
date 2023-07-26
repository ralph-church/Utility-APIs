using repair.service.unit.tests.mocks;
using System.Threading.Tasks;
using Xunit;
using repair.service.api.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace repair.service.unit.tests.controllers
{
    public class RepairRequestStageControllerTests: IClassFixture<InitializeMockRepairRequestStageFixture>
    {
        readonly InitializeMockRepairRequestStageFixture _initializeMockFixture;

        public RepairRequestStageControllerTests(InitializeMockRepairRequestStageFixture initializeMockFixture)
        {
            _initializeMockFixture = initializeMockFixture;

        }
        [Fact]
        public async Task RepairRequestStageController_Get_ReturnNotFoundResult()
        {
            var accountId = "234db243-8dfb-49e4-89ab-3d284f550113";
            var instanceId = "TMT-TEST";
            var referenceNumber = "6554";
            var eventId = "22457a13-b51a-4a02-960e-4e57a72fa447";
            var inputcontrolModels = _initializeMockFixture.MockData.Object.GetMockRepairRequestStage();
            _initializeMockFixture.MockDataRepository.Setup(x => x.FilterBy((Model => Model.AccountId == accountId
             && Model.ReferenceNumber == referenceNumber && Model.InstanceId == instanceId && Model.EventId == eventId), null, null))
                .Returns(Task.FromResult(inputcontrolModels));
            var controller = new RepairRequestStageController(_initializeMockFixture.MockRepairStageRequestService.Object,
                _initializeMockFixture.MockRepairRequestService.Object,
                new NullLogger<RepairRequestStageController>());

            var result = await controller.GetRepairRequestStages(accountId, instanceId, referenceNumber, eventId);
            var resultObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task RepairRequestStageController_Get_ReturnBadRequest()
        {
            // Arrange 
            var inputcontrolModels = _initializeMockFixture.MockData.Object.GetMockRepairRequestStage();
            _initializeMockFixture.MockDataRepository.Setup(x => x.FilterBy(null, null, null))
                .Returns(Task.FromResult(inputcontrolModels));
            var controller = new RepairRequestStageController(_initializeMockFixture.MockRepairStageRequestService.Object,
                _initializeMockFixture.MockRepairRequestService.Object,
                new NullLogger<RepairRequestStageController>());

            //Act
            var result = await controller.GetRepairRequestStages(null,null,null,null);
            var resultObjectResult = result.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }


    }
}
