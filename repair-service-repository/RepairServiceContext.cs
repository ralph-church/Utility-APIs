using MongoDB.Driver;
using repair.service.data.utilities;
using repair.service.repository.abstracts;
using System;
using System.IO;
using System.Linq;

namespace repair.service.repository
{
    /// <summary>
    /// RepairService - Mongo database context
    /// </summary>
    public class RepairServiceContext : IRepairServiceContext
    {
        private IMongoDatabase Database { get; set; }
        private MongoClient MongoClient { get; set; }


        public RepairServiceContext(IDatabaseSettings databaseSettings)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(databaseSettings.ConnectionString);
            clientSettings.SdamLogFilename = @"Logs/" + GetLogFileName();
            MongoClient = new MongoClient(clientSettings);
            Database = MongoClient.GetDatabase(databaseSettings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(Type collectionType)
        {
            return Database.GetCollection<T>(GetCollectionName(collectionType));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        //Add timestamp to sdam log
        private static string GetLogFileName()
        {
            return string.Concat(
                        Path.GetFileNameWithoutExtension("sdam-"),
                        DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff"), ".log");
        }
    }
}
