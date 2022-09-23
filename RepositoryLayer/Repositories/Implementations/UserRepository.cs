using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using RepositoryLayer.Repositories.RepositoryExtensions;

namespace RepositoryLayer.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { } 

        public async Task<PagedList<User>> GetAllUsersAsync(UserParams userParams)
        {
            var users =  await GetAllAsIQueryable()
                .Search(userParams.SearchTerm)
                .Sort(userParams.OrderBy)
                .ToListAsync();

            return PagedList<User>
                .ToPagedList(users, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<User> GetCurrentUserAsync(string name)
        {
            return await GetByCondition(x => x.Name == name).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await GetByCondition(x => x.Name == userName).SingleOrDefaultAsync();
        }

        public async Task<bool> UserExistsAsync(string name)
        {
            return await GetByCondition(x => x.Name == name).AnyAsync();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>().SingleAsync(x => x.Email == email);
        }
      
        public async Task<IEnumerable<User>> GetFollowingUsersAsync(string userName)
        {
            return await _context.Set<User>().Single(x => x.Name == userName).Following.AsQueryable().ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFollowerUsersAsync(string userName)
        {
            return await _context.Set<User>().Single(x => x.Name == userName).Followers.AsQueryable().ToListAsync();
        }
    }
}
