using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IMessageService
    {
        Task<Response<Message>> SendMessage(Message message);
        Task<Response<PagedList<Message>>> GetMessages(
            Guid currentUserId, MessageParams messageParams);

        Task<Response<PagedList<Message>>> GetSentMessages(
            Guid currentUserId, MessageParams messageParams);

        Task<Response<PagedList<Message>>> GetSentMessagesForUser(
            Guid currentUserId, Guid userId, MessageParams messageParams);

        Task<Response<PagedList<Message>>> GetMessagesForUser(
            Guid userId, Guid currentUserId, MessageParams messageParams);

        Task<Response<PagedList<Message>>> GetConversation(
            Guid senderId, Guid receiverId, MessageParams messageParams);
    }
}
