using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Pagination
{
    public abstract class RequestParams
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize { 
            get 
            { 
                return pageSize; 
            } 
            set 
            {
                if(value > maxPageSize) pageSize = maxPageSize;
                else pageSize = value;
            } 
        }
        public string? OrderBy { get; set; }
    }
}
