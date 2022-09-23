using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IUserRepository User { get; }
        ITweetRepository Tweet { get; }
        ICommentRepository Comment { get; } 
        IFollowerFollowingRepository FollowerFollowing { get; }
        ISenderRecieverRepository SenderReciever { get; }
        IMessageRepository Message { get; }
        Task<int> SaveAsync();
    }
}
