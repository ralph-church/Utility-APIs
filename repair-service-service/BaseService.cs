using AutoMapper;
using repair.service.data.abstracts;
using repair.service.repository.abstracts;
using repair.service.service.abstracts;
using repair.service.shared.paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace repair.service.service
{
    public class BaseService<TModel, TDocument> where TModel : IDocumentModel, new()
                                                      where TDocument : IDocument, new()
    {
        #region Class Variables
        private readonly IDataRepository<TDocument> _repository;
        private readonly IMapper _mapper;
        
        #endregion

        #region Constructor 
        public BaseService(IDataRepository<TDocument> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        
        public virtual async Task<IEnumerable<TModel>> FilterBy(Expression<Func<TDocument, bool>> filterExpression, int? skip, int? limit)
        {
            var result = await _repository.FilterBy(filterExpression, skip, limit);
            return _mapper.Map<IEnumerable<TDocument>, IEnumerable<TModel>>(result);
        }

        public virtual async Task<TModel> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = await _repository.FindOneAsync(filterExpression);
            return _mapper.Map<TDocument, TModel>(result);
        }

        public virtual async Task<TModel> InsertOneAsync(TModel model)
        {            
            var result = await _repository.InsertOneAsync(
                _mapper.Map<TModel, TDocument>(model));
            return _mapper.Map<TDocument, TModel>(result);
        }

        public virtual async Task<TModel> FindByIdAsync(string id)
        {
            TDocument document = await _repository.FindByIdAsync(id);
            return _mapper.Map<TDocument, TModel>(document);            
        }

        public virtual async Task<TModel> ReplaceOneAsync(TModel model)
        {            
            var result = await _repository.ReplaceOneAsync(
                        _mapper.Map<TModel, TDocument>(model));
            return _mapper.Map<TDocument, TModel>(result);
        }
        public async Task<bool> UpdateAsync(TModel model)
        {
            TDocument document = _mapper.Map<TModel, TDocument>(model);
            return await _repository.UpdateAsync(document);
        }

        public virtual async Task<TDocument> DeleteByIdAsync(string id)
        {
            return await _repository.DeleteByIdAsync(id);
        }

        public virtual async Task<PagedResponseList<TModel>> FindAsync(string groupKey, int maxRetryCount, QueryStringParams queryStringParams = null)
        {
            var result = await _repository.FindAsync(groupKey, maxRetryCount, queryStringParams);
            return _mapper.Map<PagedResponseList<TDocument>, PagedResponseList<TModel>>(result);
        }

        #endregion
    }
}

