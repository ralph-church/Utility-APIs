using integration.service.kafka.test.mocks.data;
using integration.services.kafka.shared.interfaces;
using integration.services.kafka.shared.producer;
using Moq;

namespace integration.service.kafka.test.mocks
{
    public class InitializeMockProducerFixture
    {
        public Mock<ITopicConnectionOptions> MockItopicConnectionOptions { get; }
        public Mock<ProducerMockData> MockProducerMockData { get; }
        public Mock<Producer> MockProducer { get; }
        public InitializeMockProducerFixture()
        {
            MockItopicConnectionOptions = new Mock<ITopicConnectionOptions>();
            MockProducerMockData = new Mock<ProducerMockData>();
            MockProducer = new Mock<Producer>(MockItopicConnectionOptions.Object);
        }
    }
}
