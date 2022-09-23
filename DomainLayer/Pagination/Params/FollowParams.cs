using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Pagination.Params
{
    public class FollowParams : RequestParams
    {
        public string? SearchTerm { get; set; }
    }
}
