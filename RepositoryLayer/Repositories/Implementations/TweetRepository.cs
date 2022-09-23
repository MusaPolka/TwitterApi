using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Implementations
{
    public class TweetRepository : GenericRepository<Tweet>, ITweetRepository
    {
        public TweetRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Tweet>> GetCurrentUsersTweetsOrderByPostDateAsync(string email)
        {
            return await _context.Set<Tweet>().Where(x => x.Owner.Email == email).OrderBy(x=>x.CreatedDate).ToListAsync();
        }
        public async Task<IEnumerable<Tweet>> GetUserTweetsByUserNameOrderByPostDateAsync(string userName)
        {
            return await _context.Set<Tweet>().Where(x => x.Owner.Name == userName).OrderBy(x=>x.CreatedDate).ToListAsync();
        }
    }
}
