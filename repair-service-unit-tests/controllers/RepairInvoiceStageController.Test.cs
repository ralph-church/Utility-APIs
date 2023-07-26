using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using repair.service.api.Controllers;
using repair.service.data.entities;
using repair.service.service.model;
using repair.service.unit.tests.mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace repair.service.unit.tests.controllers
{
    public class RepairInvoiceStageControllerTest : IClassFixture<InitializeMockRepairInvoiceStageFixture>
    {
        readonly InitializeMockRepairInvoiceStageFixture _initializeMockFixture;

        public RepairInvoiceStageControllerTest(InitializeMockRepairInvoiceStageFixture initializeMockFixture)
        {
            _initializeMockFixture = initializeMockFixture;

        }
        #region GetAsync

        [Fact]
        public async Task RepairInvoiceStageController_GetAll_ReturnOkResult()
        {
            var repairInvoiceStageModel = _initializeMockFixture.MockRepairInvoiceStageData.Object.MockRepairInvoiceStageCreate();
            var accountId = repairInvoiceStageModel.AccountId;
            var maxRetryCount = 0;

            _initializeMockFixture.MockDataRepository.Setup(x => x.FindAsync(accountId,maxRetryCount,null))
               .Returns(Task.FromResult(_initializeMockFixture.MockRepairInvoiceStageData.Object.MockRepairInvoiceStageGetAll()));

            var controller = new RepairInvoiceStageController(_initializeMockFixture.MockRepairInvoiceStageService.Object, new NullLogger<RepairInvoiceStageController>());

            var result = await controller.GetAllAsync(accountId, maxRetryCount,null);

            var resultObjectResult = result.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status200OK);
        }
        #endregion

        #region Create

        [Fact]
        public async Task RepairInvoiceStageController_Create_ReturnSavedRepairInvoicestageModelData()
        {
            var inputRepairInvoiceStageModel = _initializeMockFixture.MockRepairInvoiceStageData.Object.MockRepairInvoiceStageCreate();
            var mockedRepairInvoicestageModel = _initializeMockFixture.Mapper.Map<RepairInvoiceStage, RepairInvoiceStageModel>(
                inputRepairInvoiceStageModel);
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairInvoiceStage>()))
                                                .Returns(Task.FromResult(inputRepairInvoiceStageModel));
            var controller = new RepairInvoiceStageController(_initializeMockFixture.MockRepairInvoiceStageService.Object, new NullLogger<RepairInvoiceStageController>());
            //Act            
            var result = await controller.PostAsync(mockedRepairInvoicestageModel);
            var resultObjectResult = result.Result as CreatedAtRouteResult;
            var repairinvoiceStageModel = resultObjectResult.Value as RepairInvoiceModel;
            //Assert
            Assert.IsAssignableFrom<RepairInvoiceModel>(repairinvoiceStageModel);
            Assert.True(repairinvoiceStageModel.Id == inputRepairInvoiceStageModel.Id);
        }

        [Fact]
        public async Task RepairInvoiceStageController_Create_ReturnBadRequest()
        {
            // Arrange 
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(_initializeMockFixture.MockRepairInvoiceStageData.Object.MockRepairInvoiceStageCreate()))
                .Returns(Task.FromResult(_initializeMockFixture.MockRepairInvoiceStageData.Object.MockRepairInvoiceStageCreate()));

            var controller = new RepairInvoiceStageController(_initializeMockFixture.MockRepairInvoiceStageService.Object, new NullLogger<RepairInvoiceStageController>());

            //Act
            var result = await controller.PostAsync(null);
            var resultObjectResult = result.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        #endregion

        #region Update
        [Fact]
        public async Task RepairInvoiceStageController_Update_ReturnSavedRepairInvoiceModelData()
        {
            var repairInvoiceStageModel = _initializeMockFixture.MockRepairInvoiceStageData.Object.GetMockRepairInvoiceStageForToBeUpdate();
            _initializeMockFixture.MockDataRepository.Setup(x => x.UpdateAsync(repairInvoiceStageModel))
                .Returns(() =>
                {
                    var filtedRepairInvoiceStage = _initializeMockFixture.MockRepairInvoiceStageData.Object.GetExistingMockRepairInvoiceStage().FirstOrDefault(item => item.Id == repairInvoiceStageModel.Id);
                    if (filtedRepairInvoiceStage != null)
                    {
                        filtedRepairInvoiceStage.OrderId = repairInvoiceStageModel.OrderId;
                        return Task.FromResult(filtedRepairInvoiceStage != null);
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                });
            var controller = new RepairInvoiceStageController(_initializeMockFixture.MockRepairInvoiceStageService.Object, new NullLogger<RepairInvoiceStageController>());


            //Act
            var mockedRepairInvoiceStageModel = _initializeMockFixture.Mapper.Map<RepairInvoiceStage, RepairInvoiceStageModel>(
                               repairInvoiceStageModel);
            var result = await controller.Update(mockedRepairInvoiceStageModel);
            var resultObjectResult = result.Result as OkObjectResult;


            //Assert
            Assert.IsAssignableFrom<ActionResult<RepairInvoiceStageModel>>(result);
            Assert.True(mockedRepairInvoiceStageModel.OrderId == repairInvoiceStageModel.OrderId);

        }

        [Fact]
        public async Task RepairInvoiceStageController_Update_ReturnBadRequest()
        {
            // Arrange 
            var RepairInvoiceStageToBeUpdate = _initializeMockFixture.MockRepairInvoiceStageData.Object.GetMockRepairInvoiceStageForToBeUpdate();
            _initializeMockFixture.MockDataRepository.Setup(x => x.UpdateAsync(_initializeMockFixture.MockRepairInvoiceStageData.Object.GetMockRepairInvoiceStageForToBeUpdate()))
                .Returns(() =>
                {
                    var filtedIntegrationConfiguration = _initializeMockFixture.MockRepairInvoiceStageData.Object.GetExistingMockRepairInvoiceStage().FirstOrDefault(item => item.Id == RepairInvoiceStageToBeUpdate.Id);
                    filtedIntegrationConfiguration.OrderId = RepairInvoiceStageToBeUpdate.OrderId;
                    return Task.FromResult(filtedIntegrationConfiguration != null);
                });

            var controller = new RepairInvoiceStageController(_initializeMockFixture.MockRepairInvoiceStageService.Object, new NullLogger<RepairInvoiceStageController>());

            //Act
            var result = await controller.Update(null);
            var resultObjectResult = result.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        #endregion

    }
}
