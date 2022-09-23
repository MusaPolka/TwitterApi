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
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<PagedList<Message>> GetMessagesAsync(
            Guid userId, MessageParams messageParams);

        Task<PagedList<Message>> GetSentMessagesAsync(
            Guid userId, MessageParams messageParams);

        Task<PagedList<Message>> GetSentMessagesForUserAsync(
            Guid userId, Guid currentUserId, MessageParams messageParams);

        Task<PagedList<Message>> GetMessagesForUserAsync(
            Guid userId, Guid currentUserId, MessageParams messageParams);

        Task<PagedList<Message>> GetConversationAsync(
            Guid receiverId, Guid senderId, MessageParams messageParams);
    }
}
