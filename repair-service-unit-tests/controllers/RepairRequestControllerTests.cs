using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using repair.service.api.Controllers;
using repair.service.data.entities;
using repair.service.service.model;
using repair.service.shared.paging;
using repair.service.unit.tests.mocks;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace repair.service.unit.tests.controllers
{
    public class RepairRequestControllerTests: IClassFixture<InitializeMockRepairRequestFixture>
    {
        readonly InitializeMockRepairRequestFixture _initializeMockFixture;
        readonly QueryStringParams _queryStringParams;

        public RepairRequestControllerTests(InitializeMockRepairRequestFixture initializeMockFixture)
        {
            _initializeMockFixture = initializeMockFixture;
            _queryStringParams = initializeMockFixture.MockQueryStringParams.Object;
        }

        #region GetAsync

        [Fact]
        public async Task RepairRequestController_Get_ReturnOkResult()
        {
            var repairReuestModel = _initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated();
            var accountId = repairReuestModel.AccountId; ;
            var instanceId = repairReuestModel.IntegrationName;
            var orderId = repairReuestModel.OrderId;

            _initializeMockFixture.MockDataRepository.Setup(x => x.FindOneAsync(model => model.AccountId == accountId &&
                                                                                model.IntegrationName == instanceId &&
                                                                                model.OrderId == orderId))
               .Returns(Task.FromResult(_initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated()));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            var result = await controller.GetAsync(accountId, instanceId, orderId);

            var resultObjectResult = result.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RepairRequestController_Get_ReturnBadRequest()
        {
            // Arrange 
            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var result = await controller.GetAsync(null,null,null);
            var resultObjectResult = result.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task RepairRequestController_GetAllByQueryParams()
        {
            // Arrange
            string repairRequestModelAccountId = "234db243-8dfb-49e4-89ab-3d284f550113";
            var repairRequests = _initializeMockFixture.MockData.Object.GetMockRepairRequestsWithPaging();

            _initializeMockFixture.MockDataRepository.Setup(x => x.FilterByQueryParams(repairRequestModelAccountId,_queryStringParams))
                .Returns(Task.FromResult(repairRequests));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var result = await controller.GetAllByQueryParamsAsync(repairRequestModelAccountId,_queryStringParams);
            var resultObjectResult = result.Result as OkObjectResult;
            var repairRequestModels = resultObjectResult.Value as PagedResponseList<RepairRequestModel>;

            //Assert
            Assert.IsAssignableFrom<ActionResult<RepairRequestModel>>(result);
            Assert.True(repairRequestModels.Data.Count() == repairRequests.Data.Count());
        }

        [Fact]
        public async Task RepairRequestController_GetAllByQueryParams_ValidReturnsOkResult()
        {
            // Arrange
            string repairRequestModelAccountId = "234db243-8dfb-49e4-89ab-3d284f550113";
            var repairRequests = _initializeMockFixture.MockData.Object.GetMockRepairRequestsWithPaging();

            _initializeMockFixture.MockDataRepository.Setup(x => x.FilterByQueryParams(repairRequestModelAccountId, _queryStringParams))
                .Returns(Task.FromResult(repairRequests));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var result = await controller.GetAllByQueryParamsAsync(repairRequestModelAccountId, _queryStringParams);
            var resultObjectResult = result.Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status200OK);
        }
        #endregion

        #region Create

        [Fact]
        public async Task RepairRequestController_Create_ReturnSavedRepairRequestModelData()
        {
            // Arrange             
            var inputRepairRequestModel = _initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated();

            var mockedRepairRequestModel = _initializeMockFixture.Mapper.Map<RepairRequest, RepairRequestModel>(
                _initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeCreate());

            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairRequest>()))
                                                            .Returns(Task.FromResult(inputRepairRequestModel));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act            
            var result = await controller.PostAsync(mockedRepairRequestModel);
            var resultObjectResult = result.Result as  CreatedAtRouteResult;
            var RepairRequestModel = resultObjectResult.Value as RepairRequestModel;

            //Assert
            Assert.IsAssignableFrom<RepairRequestModel>(RepairRequestModel);
            Assert.True(RepairRequestModel.IntegrationName == inputRepairRequestModel.IntegrationName);
        }

        [Fact]
        public async Task RepairRequestController_CreateWithSections_ReturnSavedRepairRequestModelData()
        {
            // Arrange 
            var inputRepairRequestModel = _initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated();
            var inputRepairRequestDetailsSectionModel = inputRepairRequestModel.Details.Sections[0];

            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairRequest>()))
                                                            .Returns(Task.FromResult(inputRepairRequestModel));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var mockedRepairRequestModel = _initializeMockFixture.Mapper.Map<RepairRequest, RepairRequestModel>(_initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeCreate());
            var result = await controller.PostAsync(mockedRepairRequestModel);
            var resultObjectResult = result.Result as CreatedAtRouteResult;
            var repairRequestModel = resultObjectResult.Value as RepairRequestModel;
            var repairRequestDetailsSectionModel = repairRequestModel.Details.Sections.FirstOrDefault() as SectionModel;

            //Assert
            Assert.IsAssignableFrom<RepairRequestModel>(repairRequestModel);
            Assert.True(repairRequestModel.IntegrationName == inputRepairRequestModel.IntegrationName);
            Assert.True(repairRequestModel.Details.Sections.Count() == inputRepairRequestModel.Details.Sections.Count());
            Assert.True(repairRequestDetailsSectionModel.Title == inputRepairRequestDetailsSectionModel.Title);
        }

        [Fact]
        public async Task RepairRequestController_CreateWithSectionsAndComments_ReturnSavedRepairRequestModelData()
        {
            // Arrange 
            var inputRepairRequestModel = _initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated();
            var inputRepairRequestDetailsSectionModel = inputRepairRequestModel.Details.Sections[0];
            var inputRepairRequestDetailsCommentsModel = inputRepairRequestModel.Details.Comments[0];

            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairRequest>()))
                .Returns(Task.FromResult(_initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated()));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var mockedRepairRequestModel = _initializeMockFixture.Mapper.Map<RepairRequest, RepairRequestModel>(_initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeCreate());
            var result = await controller.PostAsync(mockedRepairRequestModel);
            var resultObjectResult = result.Result as CreatedAtRouteResult;
            var repairRequestModel = resultObjectResult.Value as RepairRequestModel;
            var repairRequestDetailSectionModel = repairRequestModel.Details.Sections.FirstOrDefault() as SectionModel;
            var repairRequestDetailCommentsModel = repairRequestModel.Details.Comments.FirstOrDefault() as CommentModel;

            //Assert
            Assert.IsAssignableFrom<RepairRequestModel>(repairRequestModel);
            Assert.True(repairRequestModel.IntegrationName == inputRepairRequestModel.IntegrationName);
            Assert.True(repairRequestDetailSectionModel.Title == inputRepairRequestDetailsSectionModel.Title);
            Assert.True(repairRequestDetailCommentsModel.CommentText == inputRepairRequestDetailsCommentsModel.CommentText);
        }
      
        [Fact]
        public async Task RepairRequestController_Create_ReturnBadRequest()
        {
            // Arrange 
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(_initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeCreate()))
                .Returns(Task.FromResult(_initializeMockFixture.MockData.Object.GetMockRepairRequestForCreated()));

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

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
        public async Task RepairRequestController_Update_ReturnsOkResult()
        {
            // Arrange 
            var repairRequestForToBeUpdate = _initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeUpdate();
            var accountId = repairRequestForToBeUpdate.AccountId;;
            var instanceId = repairRequestForToBeUpdate.IntegrationName;
            var orderId = repairRequestForToBeUpdate.OrderId;

            _initializeMockFixture.MockDataRepository.Setup(x => x.FindOneAsync(model => model.AccountId == accountId &&
                                                                                model.IntegrationName == instanceId &&
                                                                                model.OrderId == orderId))
                                                            .Returns(Task.FromResult(repairRequestForToBeUpdate));


            _initializeMockFixture.MockDataRepository.Setup(x => x.ReplaceOneAsync(_initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeUpdate()))
                                                                    .Returns(() =>
                                                                    {                   
                                                                        return Task.FromResult(repairRequestForToBeUpdate);
                                                                    });

            var mockedRepairRequestModel = _initializeMockFixture.Mapper.Map<RepairRequest, RepairRequestModel>(repairRequestForToBeUpdate);

            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);


            //Act            
            var result = await controller.PutAsync(mockedRepairRequestModel);

            //Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
               
        [Fact]
        public async Task RepairRequestController_Update_ReturnBadRequest()
        {
            // Arrange 
            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);

            //Act
            var result = await controller.PutAsync(null);
            var resultObjectResult = result.Result as BadRequestObjectResult;


            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task RepairRequestController_Update_NotFoundRequest()
        {
            // Arrange 
            var RepairRequestForToBeUpdate = _initializeMockFixture.MockData.Object.GetMockRepairRequestForToBeUpdate();            
            var controller = new RepairRequestController(_initializeMockFixture.MockRepairRequestService.Object, _initializeMockFixture.MockRepairInvoiceService.Object,new NullLogger<RepairRequestController>(), _initializeMockFixture.MockAuditService.Object);
            var RepairRequestModel = _initializeMockFixture.Mapper.Map<RepairRequest, RepairRequestModel>(RepairRequestForToBeUpdate);
            
            //Act
            var result = await controller.PutAsync(RepairRequestModel);
            var resultObjectResult = result.Result as NotFoundObjectResult;


            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.True(resultObjectResult.StatusCode == StatusCodes.Status404NotFound);
        }


        #endregion 


    }
}
