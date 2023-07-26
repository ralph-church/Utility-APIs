using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using repair.service.api.Controllers;
using repair.service.data.entities;
using repair.service.service.model;
using repair.service.unit.tests.mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace repair.service.unit.tests.controllers
{
    public class RepairInvoiceControllerTest : IClassFixture<InitializeMockRepairInvoiceFixture>
    {
        readonly InitializeMockRepairInvoiceFixture _initializeMockFixture;

        public RepairInvoiceControllerTest(InitializeMockRepairInvoiceFixture initializeMockFixture)
        {
            _initializeMockFixture = initializeMockFixture;

        }

        #region GetAsync

        [Fact]
        public async Task RepairInvoiceController_GetByOrderIdandAccountId_ReturnOkResult()
        {
            var repairReuestModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            var accountId = repairReuestModel.AccountId; 
            var orderId = repairReuestModel.OrderId;

            _initializeMockFixture.MockDataRepository.Setup(x => x.FindOneAsync(model => model.OrderId == orderId && model.AccountId == accountId))
               .Returns(Task.FromResult(_initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate()));

            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());

            var result = await controller.GetRepairInvoiceOrder(orderId,accountId);

            var resultObjectResult = result.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RepairRequestController_GetByOrderIdandAccountId_ReturnBadRequest()
        {
            // Arrange 
            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());

            //Act
            var result = await controller.GetRepairInvoiceOrder(0, null);
            var resultObjectResult = result.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        #endregion

        #region Create

        [Fact]
        public async Task RepairInvoiceController_Create_ReturnSavedRepairInvoiceModelData()
        {
            var inputRepairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            var mockedRepairInvoiceModel = _initializeMockFixture.Mapper.Map<RepairInvoice, RepairInvoiceModel>(
                inputRepairInvoiceModel);
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairInvoice>()))
                                                .Returns(Task.FromResult(inputRepairInvoiceModel));
           
            _initializeMockFixture.mockHttpRequest.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), CancellationToken.None,null))
                                               .Returns(ValueTask.FromResult(new HttpResponseMessage
                                               {
                                                   StatusCode = System.Net.HttpStatusCode.OK,
                                                   Content = new StringContent(JsonConvert.SerializeObject(inputRepairInvoiceModel))
                                               }));

            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());
            //Act            
            var result = await controller.PostAsync(mockedRepairInvoiceModel);
            var resultObjectResult = result.Result as CreatedAtRouteResult;
            var repairinvoiceModel = resultObjectResult.Value as RepairInvoiceModel;
            //Assert
            Assert.IsAssignableFrom<RepairInvoiceModel>(repairinvoiceModel);
            Assert.True(repairinvoiceModel.Id == inputRepairInvoiceModel.Id);
        }

        [Fact]
        public async Task RepairInvoiceController_Create_ReturnBadRequest()
        {
            // Arrange 
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(_initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate()))
                .Returns(Task.FromResult(_initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate()));

            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());

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
        public async Task RepairInvoiceController_Update_ReturnSavedRepairInvoiceModelData()
        {
            var repairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.GetMockRepairInvoiceForToBeUpdate();
            _initializeMockFixture.MockDataRepository.Setup(x => x.UpdateAsync(repairInvoiceModel))
                .Returns(() =>
                {
                    var filtedRepairInvoice = _initializeMockFixture.MockRepairInvoiceData.Object.GetExistingMockRepairInvoice().FirstOrDefault(item => item.Id == repairInvoiceModel.Id);
                    if (filtedRepairInvoice != null)
                    {
                        filtedRepairInvoice.OrderId = repairInvoiceModel.OrderId;
                        return Task.FromResult(filtedRepairInvoice != null);
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                });
            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());


            //Act
            var mockedRepairInvoiceModel = _initializeMockFixture.Mapper.Map<RepairInvoice, RepairInvoiceModel>(
                               repairInvoiceModel);
            var result = await controller.Update(mockedRepairInvoiceModel);
            var resultObjectResult = result.Result as OkObjectResult;


            //Assert
            Assert.IsAssignableFrom<ActionResult<RepairInvoiceModel>>(result);
            Assert.True(mockedRepairInvoiceModel.OrderId == repairInvoiceModel.OrderId);

        }

        [Fact]
        public async Task RepairInvoiceController_Update_ReturnBadRequest()
        {
            // Arrange 
            var RepairInvoiceToBeUpdate = _initializeMockFixture.MockRepairInvoiceData.Object.GetMockRepairInvoiceForToBeUpdate();
            _initializeMockFixture.MockDataRepository.Setup(x => x.UpdateAsync(_initializeMockFixture.MockRepairInvoiceData.Object.GetMockRepairInvoiceForToBeUpdate()))
                .Returns(() =>
                {
                    var filtedIntegrationConfiguration = _initializeMockFixture.MockRepairInvoiceData.Object.GetExistingMockRepairInvoice().FirstOrDefault(item => item.Id == RepairInvoiceToBeUpdate.Id);
                    filtedIntegrationConfiguration.OrderId = RepairInvoiceToBeUpdate.OrderId;
                    return Task.FromResult(filtedIntegrationConfiguration != null);
                });

            var controller = new RepairInvoiceController(_initializeMockFixture.MockRepairInvoiceService.Object, new NullLogger<RepairInvoiceController>());

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
