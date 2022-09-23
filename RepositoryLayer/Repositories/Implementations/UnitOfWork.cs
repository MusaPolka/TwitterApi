using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Implementations
{
    public class UnitOfWork : Interfaces.IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            User = new UserRepository(_context);
            Tweet = new TweetRepository(_context);
            Comment = new CommentRepository(_context);
            SenderReciever = new SenderRecieverRepository(_context);
            FollowerFollowing = new FollowerFollowingRepository(_context);
            Message = new MessageRepository(_context);
        }

        public IUserRepository User { get; private set; }

        public ITweetRepository Tweet { get; private set; }

        public ICommentRepository Comment { get; private set; }

        public IFollowerFollowingRepository FollowerFollowing { get; private set;}

        public ISenderRecieverRepository SenderReciever { get; private set; }

        public IMessageRepository Message { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
