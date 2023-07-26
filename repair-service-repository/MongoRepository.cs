using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using repair.service.repository.abstracts;
using repair.service.data.abstracts;
using repair.service.shared.paging;


namespace repair.service.repository
{

    public class MongoRepository<TDocument> : IDataRepository<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IRepairServiceContext dbContext)
        {
            _collection = dbContext.GetCollection<TDocument>(typeof(TDocument));
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            try
            {
                return _collection.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<IEnumerable<TDocument>> FilterBy(Expression<Func<TDocument, bool>> filterExpression, int? skip, int? limit)
        {
            try
            {
                var result = filterExpression != null
                       ? (skip == null || limit == null
                           ? _collection.Find(filterExpression).ToList()
                           : _collection.Find(filterExpression).Skip(skip).Limit(limit).ToList())
                       : _collection.Find(s => true).ToList();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<PagedResponseList<TDocument>> FilterByQueryParams(string accountId, QueryStringParams queryStringParams)
        {
            try
            {
                var builder = Builders<TDocument>.Filter;
                IList<FilterDefinition<TDocument>> filters = new List<FilterDefinition<TDocument>>();

                if (queryStringParams.Filter == null)
                {
                    queryStringParams.Filter = new List<Filtering>();
                }

                if (!string.IsNullOrEmpty(accountId))
                {
                    queryStringParams.Filter.Add(new Filtering() { Field = "AccountId", Operator = "eq", Value = accountId });
                }

                filters = PagedResponseList<TDocument>.FilterParameterList(queryStringParams);

                var filterConcat = builder.And(filters);
                var result = PagedResponseList<TDocument>.ToPagedResponseList(_collection.Find(filterConcat),
                queryStringParams != null ? queryStringParams.PageNo : 0, queryStringParams != null ? queryStringParams.PageSize : 0,
                queryStringParams.SortBy != null ? queryStringParams : null);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // TODO: We can modify based on the requirement.
        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            try
            {
                return filterExpression != null && projectionExpression != null
                        ? _collection.Find(filterExpression).Project(projectionExpression).ToList()
                        : (IEnumerable<TProjected>)_collection.Find(s => true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async virtual Task<IEnumerable<TDocument>> FindAll(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                var result = filterExpression != null ? _collection.Find(filterExpression).ToList() : _collection.Find(s => true).ToList();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                return filterExpression != null
                       ? _collection.Find(filterExpression).FirstOrDefault()
                       : _collection.Find(s => true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                var result = filterExpression != null
                           ? _collection.Find(filterExpression).FirstOrDefaultAsync()
                           : _collection.Find(s => true).FirstOrDefaultAsync();
                return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDocument FindById(string id)
        {
            try
            {
                return !string.IsNullOrEmpty(id)
                        ? _collection.Find(doc => doc.Id == id).FirstOrDefault()
                        : _collection.Find(doc => true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<TDocument> FindByIdAsync(string id)
        {
            try
            {
                return !string.IsNullOrEmpty(id)
                       ? await _collection.Find(doc => doc.Id == id).FirstOrDefaultAsync()
                       : await _collection.Find(doc => true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDocument InsertOne(TDocument document)
        {
            try
            {
                if (document != null)
                {
                    _collection.InsertOne(document);
                    return document;
                }
                else
                {
                    return default(TDocument);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<TDocument> InsertOneAsync(TDocument document)
        {
            try
            {
                if (document != null)
                {
                    await _collection.InsertOneAsync(document);
                    return document;
                }
                else
                {

                    return default(TDocument);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
        {
            try
            {
                if (documents != null && documents.Count > 0)
                {
                    _collection.InsertMany(documents);
                    return documents;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async virtual Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents)
        {
            try
            {
                if (documents != null && documents.Count > 0)
                {
                    await _collection.InsertManyAsync(documents);
                    return documents;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDocument ReplaceOne(TDocument document)
        {
            try
            {
                if (document != null)
                {
                    var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
                    var result = _collection.FindOneAndReplace(filter, document);
                    return result;
                }
                else
                {
                    return default(TDocument);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async virtual Task<TDocument> ReplaceOneAsync(TDocument document)
        {
            try
            {
                if (document != null)
                {
                    var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
                    var result = await _collection.FindOneAndReplaceAsync(filter, document);
                    return result;
                }
                else
                {
                    return default(TDocument);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                if (filterExpression != null)
                {
                    _collection.FindOneAndDelete(filterExpression);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async virtual Task<bool> UpdateAsync(TDocument document)
        {
            var updateResult = await _collection
                                        .ReplaceOneAsync(filter: item => item.Id == document.Id, replacement: document);

            return updateResult.IsAcknowledged
                    && updateResult.MatchedCount > 0;
        }
        public async Task<TDocument> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                return filterExpression != null
                         ? await _collection.FindOneAndDeleteAsync(filterExpression)
                         : default(TDocument);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteById(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    _collection.FindOneAndDelete(doc => doc.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TDocument> DeleteByIdAsync(string id)
        {
            try
            {
                return !string.IsNullOrEmpty(id)
                       ? await _collection.FindOneAndDeleteAsync(doc => doc.Id == id)
                       : default(TDocument);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                if (filterExpression != null)
                {
                    _collection.DeleteMany(filterExpression);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            try
            {
                return filterExpression != null
                        ? await _collection.DeleteManyAsync(filterExpression)
                        : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PagedResponseList<TDocument>> FindAsync(string groupKey, int maxRetryCount, QueryStringParams queryStringParams)
        {

            if (string.IsNullOrEmpty(groupKey))
            {
                var result = PagedResponseList<TDocument>.ToPagedResponseList(_collection, queryStringParams);
                return await Task.FromResult(result);
            }
            else
            {
               var result= PagedResponseList<TDocument>.ToPagedResponseList(GetGroupedDocuments(groupKey, (maxRetryCount == 0 ? 3 : maxRetryCount)).Result);
                return await Task.FromResult(result);
            }
        }

        private async Task<IEnumerable<TDocument>> GetGroupedDocuments(string groupKey, int maxRetryCount)
        {

            groupKey = "$" + groupKey;

            // Apply Aggregation 
            // Step 1 - Fecth repair invoice stages data which is having retry count less than 4
            // Step 2 - Group  repair invoice stages by account id .This step is very important to avoid performance issue in on prem server api.
            // Step 3 - Fecth single repair invoice stages data in every group 
            PipelineDefinition<TDocument, BsonDocument> pipeline = new BsonDocument[]
                         {
                                     new BsonDocument("$match",
                                                new BsonDocument(
                                                        "retry_count",
                                                        new BsonDocument( "$lt" , maxRetryCount))
                                                      ),
                                      new BsonDocument("$group",
                                               new BsonDocument
                                                    {
                                                         { "_id", groupKey },
                                                         { "data",
                                                        new BsonDocument("$first","$$ROOT") }
                                                     }),
                         };

            var repairInvoices = new List<TDocument>();

            var options = new AggregateOptions()
            {
                AllowDiskUse = false
            };

            using (var cursor = await _collection.AggregateAsync(pipeline, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        var repairInvoiceAsBsonDocument = document.GetElement("data").Value.AsBsonDocument;
                        var repairInvoice = BsonSerializer.Deserialize<TDocument>(repairInvoiceAsBsonDocument);
                        repairInvoices.Add(repairInvoice);
                    }
                }
            }
            return repairInvoices;
        }
    }
}
