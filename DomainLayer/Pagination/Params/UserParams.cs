using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Pagination.Params
{
    public class UserParams : RequestParams
    {
        public UserParams()
        {
            OrderBy = "name";
        }
        public string? SearchTerm { get; set; }
    }
}
