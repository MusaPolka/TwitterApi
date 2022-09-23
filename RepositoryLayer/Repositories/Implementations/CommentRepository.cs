using DomainLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Implementations
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context) { }
    }
}
