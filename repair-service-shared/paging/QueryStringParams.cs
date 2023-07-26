using System.Collections.Generic;

namespace repair.service.shared.paging
{
    public class QueryStringParams
    {
        private int _maxPageSize = 50;
        public virtual int maxPageSize
        {
            get => _maxPageSize;
            set => _maxPageSize = (value < maxPageSize) ? maxPageSize : value;
        }
        public virtual int PageNo { get; set; } = 1;
        private int _pageSize = 10;
        public virtual int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public virtual List<Sorting> SortBy { get; set; }

        public virtual List<Filtering> Filter { get; set; }
    }

    public class Sorting
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
    public class Filtering
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
