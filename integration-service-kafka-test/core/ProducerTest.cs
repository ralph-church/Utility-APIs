using Confluent.Kafka;
using integration.service.kafka.test.mocks;
using integration.services.kafka.shared.producer;
using Moq;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace integration.service.kafka.test.core
{
    public class ProducerTest : IClassFixture<InitializeMockProducerFixture>
    {
        readonly InitializeMockProducerFixture _initializeMockProducerFixture;
        public Producer _producer;
        public ProducerTest(InitializeMockProducerFixture initializeMockProducerFixture)
        {
            _initializeMockProducerFixture = initializeMockProducerFixture;
        }

        [Fact]
        public async void kafka_PublishAsync_Return_Success()
        {
            // Arrange  
            _initializeMockProducerFixture.MockProducer.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(_initializeMockProducerFixture.MockProducerMockData.Object.ProduceDeliveryDetails());

            // Act            
            var deliveryReport = await _initializeMockProducerFixture.MockProducer.Object.PublishAsync(It.IsAny<string>(), It.IsAny<object>());

            // Assert
            Assert.True(deliveryReport.Status == PersistenceStatus.Persisted);           
        }

        [Fact]
        public async Task kafka_PublishAsync_Fails_LogsErrorWithException()
        {
            try
            {
                //Arrange           
                _initializeMockProducerFixture.MockProducer.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>()))
               .Throws(new TaskCanceledException());

                //Act
                await _initializeMockProducerFixture.MockProducer.Object.PublishAsync(It.IsAny<string>(), It.IsAny<object>());
            }
            catch (Exception e)
            {
                Assert.True(e.GetType() == typeof(TaskCanceledException));
            }
        }
    }
}
