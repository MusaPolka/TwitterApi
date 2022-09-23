using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Repositories.Implementations;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInteractionService _userInteractionService;

        public MessageService(IUnitOfWork unitOfWork, IUserInteractionService userInteractionService)
        {
            _unitOfWork = unitOfWork;
            _userInteractionService = userInteractionService;
        }

        public async Task<Response<Message>> SendMessage(Message message)
        {
            if (!_userInteractionService.isPublicUser(message.Recipient))
            {
                if(!await _userInteractionService.isAlreadyFollowed(message.RecipientId, message.SenderId))
                {
                    return new Response<Message>(false,
                        $"You cant send messages to private users, untill you are followed them",
                        null);
                }
            }

            _unitOfWork.Message.Add(message);

            await _unitOfWork.SaveAsync();

            return new Response<Message>(true, 
                $"Message have successfully sent to {message.RecipientUsername}", 
                message);
        }

        public async Task<Response<PagedList<Message>>> GetMessages(Guid currentUserId, 
            MessageParams messageParams)
        {
            var messages = await _unitOfWork.Message
                .GetMessagesAsync(currentUserId, messageParams);

            if(messages == null)
            {
                return new Response<PagedList<Message>>(false,
                $"You do not have any messages",
                null);
            }

            return new Response<PagedList<Message>>(true,
                $"Here is your messages",
                messages);
        }
        public async Task<Response<PagedList<Message>>> GetSentMessages(
            Guid currentUserId, MessageParams messageParams)
        {
            var messages = await _unitOfWork.Message
                .GetSentMessagesAsync(currentUserId, messageParams);

            if (messages == null)
            {
                return new Response<PagedList<Message>>(false,
                $"You do not have any send messages",
                null);
            }

            return new Response<PagedList<Message>>(true,
                $"Here is your send messages",
                messages);
        }
        public async Task<Response<PagedList<Message>>> GetSentMessagesForUser(
            Guid currentUserId, Guid userId, MessageParams messageParams)
        {
            var messages = await _unitOfWork.Message
                .GetSentMessagesForUserAsync(userId, currentUserId, messageParams);

            if (messages == null)
            {
                return new Response<PagedList<Message>>(false,
                $"You do not have any send messages to this user",
                null);
            }

            return new Response<PagedList<Message>>(true,
                $"Here is your send messages to this user",
                messages);
        }

        public async Task<Response<PagedList<Message>>> GetMessagesForUser(
            Guid userId, Guid currentUserId, MessageParams messageParams)
        {
            var messages = await _unitOfWork.Message
                .GetMessagesForUserAsync(userId, currentUserId, messageParams);

            if(messages == null)
            {
                return new Response<PagedList<Message>>(false,
                    $"You do not have any messages from this user",
                    null);
            }

            return new Response<PagedList<Message>>(true,
                    $"Here are all the messages from this user",
                    messages);
        }

        public async Task<Response<PagedList<Message>>> GetConversation(
            Guid senderId, Guid receiverId, MessageParams messageParams)
        {
            var messages = await _unitOfWork.Message
                .GetConversationAsync(receiverId, senderId, messageParams);

            if (messages == null)
            {
                return new Response<PagedList<Message>>(false,
                    $"You do not have any conversation with this user",
                    null);
            }

            return new Response<PagedList<Message>>(true,
                    $"Here is the conversation with this user",
                    messages);
        }
    }
}
