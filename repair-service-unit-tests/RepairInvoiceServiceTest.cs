using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using repair.service.data.entities;
using repair.service.service;
using repair.service.service.model;
using repair.service.unit.tests.mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace repair.service.unit.tests
{
    public class RepairInvoiceServiceTest : IClassFixture<InitializeMockRepairInvoiceFixture>
    {
        readonly InitializeMockRepairInvoiceFixture _initializeMockFixture;

        public RepairInvoiceServiceTest(InitializeMockRepairInvoiceFixture initializeMockFixture)
        {
            _initializeMockFixture = initializeMockFixture;

        }

        [Fact]
        public void Create_ReturnSavedRepairInvoiceModelData_Success()
        {
            var inputRepairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            var mockedRepairInvoiceModel = _initializeMockFixture.Mapper.Map<RepairInvoice, RepairInvoiceModel>(
                inputRepairInvoiceModel);
            _initializeMockFixture.MockDataRepository.Setup(x => x.InsertOneAsync(It.IsAny<RepairInvoice>()))
                                                .Returns(Task.FromResult(inputRepairInvoiceModel));
            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.InsertOneAsync(mockedRepairInvoiceModel);
            //Assert
            Assert.NotEmpty(results.Result.Id);
            Assert.True(results.IsCompletedSuccessfully);
        }

        //[Fact]
        // Invalid Test
        public void Create_ReturnSavedRepairInvoiceModelData_Null()
        {

            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.InsertOneAsync(null).Result;
            //Assert
            Assert.Null(results);
        }

        [Fact]
        public void RepairInvoice_Get_ReturnOkResult()
        {
            var repairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            var orderId = repairInvoiceModel.OrderId;
            var accountId = repairInvoiceModel.AccountId;
            _initializeMockFixture.MockDataRepository.Setup(x => x.FindOneAsync(s=> s.OrderId == orderId && s.AccountId == accountId))
                                               .Returns(Task.FromResult(repairInvoiceModel));
            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.FindOneAsync(orderId, accountId);
            Assert.NotEmpty(results.Result.Id);
            Assert.True(results.IsCompletedSuccessfully);

        }

        [Fact]
        public void RepairInvoice_Get_ReturnBadResult()
        {
            var repairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.FindOneAsync(null);
            Assert.Null(results.Result);

        }

        [Fact]
        public void RepairInvoice_Update_Success()
        {
            var repairInvoiceModel = _initializeMockFixture.MockRepairInvoiceData.Object.MockRepairInvoiceCreate();
            repairInvoiceModel.Status = "COMPLETE";
            repairInvoiceModel.CompletedDate = DateTime.UtcNow;
             var mockedRepairInvoiceModel = _initializeMockFixture.Mapper.Map<RepairInvoice, RepairInvoiceModel>(
                                            repairInvoiceModel);
            _initializeMockFixture.MockDataRepository.Setup(x => x.ReplaceOneAsync(It.IsAny<RepairInvoice>()))
                                                .Returns(Task.FromResult(repairInvoiceModel));
            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.ReplaceOneAsync(mockedRepairInvoiceModel);
            Assert.Equal(results.Result.Status, repairInvoiceModel.Status);

        }

        //[Fact]
        // Invalid Test
        public void RepairInvoice_Update_BadRequest()
        {

            RepairInvoiceService repairInvoiceService = GetRepairInvoiceService();
            var results = repairInvoiceService.ReplaceOneAsync(null).Result;
            //Assert
            Assert.Null(results);
        }
        
        private RepairInvoiceService GetRepairInvoiceService()
        {
            return new RepairInvoiceService(
                _initializeMockFixture.MockLogger.Object,
                _initializeMockFixture.MockDataRepository.Object,
                _initializeMockFixture.Mapper,
                _initializeMockFixture.mockHttpRequest.Object,
                _initializeMockFixture.mockTtcServicesConfig.Object);
        }
    }
}
