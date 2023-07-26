using repair.service.repository.abstracts;

namespace repair.service.repository.config
{
   
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }      
        public DatabaseSettings() { }
    }
}
