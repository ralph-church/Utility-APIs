using repair.service.data.abstracts;
using repair.service.shared.paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace repair.service.service.abstracts
{
    public interface IBaseService<TModel, TDocument> where TModel : IDocumentModel, new()
                                                      where TDocument : IDocument, new()
    {
        Task<TModel> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TModel>> FilterBy(Expression<Func<TDocument, bool>> filterExpression, int? skip, int? limit);
        Task<TModel> InsertOneAsync(TModel model);
        Task<TModel> FindByIdAsync(string id);
        Task<PagedResponseList<TModel>> FindAsync(string groupKey, int maxRetryCount, QueryStringParams queryStringParams = null);
        Task<TModel> ReplaceOneAsync(TModel model);
        Task<bool> UpdateAsync(TModel model);
        Task<TDocument> DeleteByIdAsync(string id);
    }
}
