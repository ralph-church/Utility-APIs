namespace repair.service.repository.abstracts
{
    public interface IDatabaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }       
    }
}
