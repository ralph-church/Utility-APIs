using MongoDB.Driver;
using System;

namespace repair.service.repository.abstracts
{
    public interface IRepairServiceContext
    {
       IMongoCollection<T> GetCollection<T>(Type collectionType);
    }
}
