using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User> 
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetFollowingUsersAsync(string userName);
        Task<IEnumerable<User>> GetFollowerUsersAsync(string userName);
        Task<PagedList<User>> GetAllUsersAsync(UserParams userParams);
        Task<User> GetCurrentUserAsync(string name);
        Task<User> GetUserByNameAsync(string userName);
        Task<bool> UserExistsAsync(string email);
    }
}
