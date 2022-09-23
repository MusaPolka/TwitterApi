using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
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
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<PagedList<Message>> GetMessagesAsync(Guid userId, 
            MessageParams messageParams)
        {
            var messages =  await GetByCondition(x => x.RecipientId == userId).ToListAsync();

            return PagedList<Message>
                .ToPagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }
        public async Task<PagedList<Message>> GetSentMessagesAsync(Guid userId, 
            MessageParams messageParams)
        {
            var messages =  await GetByCondition(x => x.SenderId == userId).ToListAsync();

            return PagedList<Message>
                .ToPagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<PagedList<Message>> GetMessagesForUserAsync(
            Guid userId, Guid currentUserId, MessageParams messageParams)
        {
            var messages =  await GetByCondition(
                x => x.SenderId == userId && x.RecipientId == currentUserId)
                .ToListAsync();

            return PagedList<Message>
                .ToPagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }
        public async Task<PagedList<Message>> GetSentMessagesForUserAsync(
            Guid userId, Guid currentUserId, MessageParams messageParams)
        {
            var messages = await GetByCondition(
                x => x.SenderId == currentUserId && x.RecipientId == userId)
                .ToListAsync();

            return PagedList<Message>
                .ToPagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<PagedList<Message>> GetConversationAsync(
            Guid receiverId, Guid senderId, MessageParams messageParams)
        {
            var messages = await GetByCondition(
                x => x.RecipientId == receiverId && x.SenderId == senderId
                || x.RecipientId == senderId && x.SenderId == receiverId)
                .OrderBy(x => x.CreatedDate).ToListAsync();

            return PagedList<Message>
                .ToPagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }
    }
}
