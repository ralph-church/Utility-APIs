namespace integration.services.kafka.shared.interfaces
{
    public interface ITopicConnectionOptions
    {
        public string Host { get; set; }
        public string EntityName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Key { get; set; }
        public string InboundSubscription { get; set; }
        public int? MessageMaxBytes { get; set; }
    }
}
