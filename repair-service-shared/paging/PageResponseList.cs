using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace repair.service.shared.paging
{
    public class PagedResponseList<T>
    {
        public List<T> Data { get; set; }
        public PagingInfo<T> Paging { get; set; }

        public PagedResponseList(List<T> items, long count, int pageNumber, int pageSize)
        {
            Data = items;
            Paging = new PagingInfo<T>
            {
                PageNo = pageNumber,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(count / (double)pageSize),
                TotalRecordCount = count
            };
        }
        public static PagedResponseList<T> ToPagedResponseList(IFindFluent<T, T> source, int pageNumber, int pageSize, QueryStringParams param = null)
        {
            IFindFluent<T, T> items = null;
            var result = new PagedResponseList<T>(null, 0, 0, 0);
            if (source != null)
            {
                List<string> sortAdd = new List<string>();
                var sortFields = string.Empty;
                var count = source.CountDocuments();
                if (param != null)
                {
                    foreach (var sorting in param.SortBy)
                    {
                        var sort = sorting.Dir.Trim().ToLower().StartsWith("asc") ? 1 : -1;
                        sortAdd.Add(sorting.Field.Trim() + ": " + sort);
                        sortFields = string.Join(",", sortAdd.ToArray());
                    }
                }
                if (sortFields != "")
                {
                    items = pageNumber <= 0 ? source : source.Skip((pageNumber - 1) * pageSize).Limit(pageSize).Sort("{ " + sortFields + "}");
                }
                else
                {
                    items = pageNumber <= 0 ? source : source.Skip((pageNumber - 1) * pageSize).Limit(pageSize);
                }
                result = new PagedResponseList<T>(items.ToList(), count, pageNumber > 0 ? pageNumber : 1, pageSize > 0 ? pageSize : Convert.ToInt32(count));
            }
            return result;
        }

        public static PagedResponseList<T> ToPagedResponseList(IMongoCollection<T> source, QueryStringParams param)
        {

            var result = new PagedResponseList<T>(null, 0, 0, 0);
            if (source != null)
            {
                var builder = Builders<T>.Filter;
                IFindFluent<T, T> items = null;
                IFindFluent<T, T> sourceFiltered = null;
                var pageNumber = param != null ? param.PageNo : 0;
                var pageSize = param != null ? param.PageSize : 0;
                var count = source.Find(s => true).CountDocuments();
                IList<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();
                var sortFields = string.Empty;
                List<string> sortAdd = new List<string>();
                if (param != null && param.Filter != null && param.Filter.Count > 0)
                {
                    filters = FilterParameterList(param);
                    var filterConcat = builder.And(filters);
                    sourceFiltered = source.Find(filterConcat);
                    if (sourceFiltered != null)
                    {
                        items = pageNumber <= 0 ? sourceFiltered : sourceFiltered.Skip((pageNumber - 1) * pageSize).Limit(pageSize);
                    }
                }
                if (param != null && param.SortBy != null && param.SortBy.Count > 0)
                {
                    foreach (var sorting in param.SortBy)
                    {
                        var sort = sorting.Dir.Trim().ToLower().StartsWith("asc") ? 1 : -1;
                        sortAdd.Add(sorting.Field.Trim() + ": " + sort);
                        sortFields = string.Join(",", sortAdd.ToArray());
                    }
                    if (sourceFiltered == null)
                    {
                        items = pageNumber <= 0 ? source.Find(s => true) : source.Find(s => true).Skip((pageNumber - 1) * pageSize).Limit(pageSize).Sort("{ " + sortFields + "}");
                    }
                    else
                    {
                        items = items.Sort("{ " + sortFields + "}");
                    }

                }
                if (param == null || (param != null && param.Filter == null && param.SortBy == null))
                {
                    items = pageNumber <= 0 ? source.Find(s => true) : source.Find(s => true).Skip((pageNumber - 1) * pageSize).Limit(pageSize);
                }
                result = new PagedResponseList<T>(items.ToList(), count, pageNumber > 0 ? pageNumber : 1, pageSize > 0 ? pageSize : Convert.ToInt32(count));
            }
            return result;
        }

        public PagedResponseList() { }

        public static PagedResponseList<T> ToPagedResponseList(IEnumerable<T> items)
        {
            int count = items != null ? items.Count() : 0;
            var result =  new PagedResponseList<T>(items.ToList(), count, 1, count);
            return result;
        }

        public static IList<FilterDefinition<T>> FilterParameterList(QueryStringParams param)
        {
            var builder = Builders<T>.Filter;
            IList<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();
            foreach (var filtering in param.Filter)
            {
                switch (filtering.Operator.ToLower())
                {
                    case "eq":
                        filters.Add(builder.Eq(filtering.Field, filtering.Value));
                        break;
                    case "neq":
                        filters.Add(builder.Ne(filtering.Field, filtering.Value));
                        break;
                    case "contains":
                        filters.Add(builder.Regex(filtering.Field, ".*" + filtering.Value + ".*"));
                        break;
                    case "startswith":
                        filters.Add(builder.Regex(filtering.Field, "^" + filtering.Value));
                        break;
                    case "endswith":
                        filters.Add(builder.Regex(filtering.Field, filtering.Value + "$"));
                        break;
                    case "isnull":
                        filters.Add(builder.Eq(filtering.Field, BsonNull.Value));
                        break;
                    case "isnotnull":
                        filters.Add(builder.Ne(filtering.Field, BsonNull.Value));
                        break;
                    case "gte":
                        filters.Add(builder.Gte(filtering.Field, filtering.Value));
                        break;
                    case "lte":
                        filters.Add(builder.Lte(filtering.Field, filtering.Value));
                        break;
                }
            }
            return filters;
        }

    }

    public class PagingInfo<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public long TotalRecordCount { get; set; }

    }
}
