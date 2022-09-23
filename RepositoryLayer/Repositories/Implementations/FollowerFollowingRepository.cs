using DomainLayer.Entities;
using DomainLayer.Pagination;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Implementations
{
    public class FollowerFollowingRepository : GenericRepository<FollowerFollowing>, IFollowerFollowingRepository
    {
        public FollowerFollowingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
