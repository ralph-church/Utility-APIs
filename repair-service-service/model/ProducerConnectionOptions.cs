using integration.services.kafka.shared.interfaces;

namespace repair.service.service.model
{
    public class ProducerConnectionOptions : ITopicConnectionOptions
    {
        public string Host { get; set; }
        public string EntityName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Key { get; set; }
        public string InboundSubscription { get; set; }
        public string TopicType { get; set; }
        public int? MessageMaxBytes { get; set; }
    }
}
