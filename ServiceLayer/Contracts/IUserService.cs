using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IUserService
    {
        Task<PagedList<User>> GetAllUsersAsync(UserParams userParams);
        Task<User> GetCurrentUserAsync(string name);
        Task<User> GetUserByNameAsync(string userName);
        Task<IEnumerable<Tweet>> GetFollowingUserTweetsAsync(string userName);
        Task Save();
    }
}
