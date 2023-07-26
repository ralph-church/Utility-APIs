using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using repair.service.data.abstracts;
using repair.service.shared.paging;

namespace repair.service.repository.abstracts
{
    public interface IDataRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        Task<IEnumerable<TDocument>> FilterBy(Expression<Func<TDocument, bool>> filterExpression, int? skip, int? limit);
        Task<PagedResponseList<TDocument>> FilterByQueryParams(string accountId, QueryStringParams queryStringParams);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);
        Task<IEnumerable<TDocument>> FindAll(Expression<Func<TDocument, bool>> filterExpression);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        
        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        TDocument InsertOne(TDocument document);

        Task<TDocument> InsertOneAsync(TDocument document);

        ICollection<TDocument> InsertMany(ICollection<TDocument> documents);

        Task<ICollection<TDocument>> InsertManyAsync(ICollection<TDocument> documents);

        TDocument ReplaceOne(TDocument document);
        Task<bool> UpdateAsync(TDocument document);
        Task<TDocument> ReplaceOneAsync(TDocument document);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById(string id);
        Task<TDocument> DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task<MongoDB.Driver.DeleteResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<PagedResponseList<TDocument>> FindAsync(string groupKey, int maxRetryCount, QueryStringParams queryStringParams);


    }
}
