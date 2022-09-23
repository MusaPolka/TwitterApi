using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Moq;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class MessageServiceTests
    {
        private readonly MessageService _messageSut;
        private readonly UserInteractionService _userInteractionSut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        public MessageServiceTests()
        {
            _userInteractionSut = new UserInteractionService(_unitOfWorkMock.Object, _mapper.Object);
            _messageSut = new MessageService(_unitOfWorkMock.Object, _userInteractionSut);
        }

        [Fact]
        public async Task SendMessage_ShouldSendCorrectMessage()
        {
            var message = new Message()
            {
                Content = "Test message",
                Sender = new User() { Name = "senderTest", Email = "senderEmail@test.com" },
                SenderUsername = "senderTest",
                Recipient = new User() { Name = "recipientTest", Email = "recipientEmail@test.com" },
                RecipientUsername = "recipientTest",
            };

            _unitOfWorkMock.Setup(x => x.Message.Add(message));

            var response = await _messageSut.SendMessage(message);

            Assert.True(response.Success);
            Assert.Equal(message.Content, response.Content.Content);
            Assert.Equal(message.SenderUsername, response.Content.SenderUsername);
            Assert.Equal(message.RecipientUsername, response.Content.RecipientUsername);
        }
        [Fact]
        public async Task SendMessage_ShouldThrow_NullReferenceException_WhenMessageIsNull()
        {
            var message = new Message()
            {
                Content = "Test message",
                Sender = new User() { Name = "senderTest", Email = "senderEmail@test.com" },
                SenderUsername = "senderTest",
                Recipient = new User() { Name = "recipientTest", Email = "recipientEmail@test.com" },
                RecipientUsername = "recipientTest",
            };

            _unitOfWorkMock.Setup(x => x.Message.Add(message));

            await Assert.ThrowsAsync<NullReferenceException>(() => _messageSut.SendMessage(null));
        }

        [Fact]
        public async Task GetMessages_ShouldReturnCorrentNumberOfMessages()
        {
            var currentUserId = Guid.NewGuid();

            var messages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test message",
                    Sender = new User() { Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() { Id = currentUserId, Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                }
            };

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var pagedMessages = PagedList<Message>.ToPagedList(
                messages, messageParams.PageNumber, messageParams.PageSize);

            _unitOfWorkMock.Setup(x => x.Message.GetMessagesAsync(currentUserId, messageParams)).ReturnsAsync(pagedMessages);

            var response = await _messageSut.GetMessages(currentUserId, messageParams);

            Assert.True(response.Success);
            Assert.Equal(messages.Count(), response.Content.Count());
        }

        [Fact]
        public async Task GetMessages_ShouldReturn_UnsuccessfullResnpose_WhenUserDoesntHaveAnyMessages()
        {
            var currentUserId = Guid.NewGuid();

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(x => x.Message.GetMessagesAsync(currentUserId, messageParams)).ReturnsAsync(() => null);

            var response = await _messageSut.GetMessages(currentUserId, messageParams);

            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldReturnCorrentNumberOfMessages()
        {
            var userId = Guid.NewGuid();
            var currentUserId = Guid.NewGuid();

            var messages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test message",
                    Sender = new User() {Id = userId, Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() { Id = currentUserId, Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                }
            };

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var pagedMessages = PagedList<Message>.ToPagedList(
                messages, messageParams.PageNumber, messageParams.PageSize);

            _unitOfWorkMock.Setup(x => x.Message.GetMessagesForUserAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(pagedMessages);

            var response = await _messageSut.GetMessagesForUser(
                userId, currentUserId, messageParams);

            Assert.True(response.Success);
            Assert.Equal(messages.Count(), response.Content.Count());
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldReturn_UnsuccessfullResnpose_WhenUserDoesntHaveAnyMessages()
        {
            var currentUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(x => x.Message.GetMessagesForUserAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(() => null);

            var response = await _messageSut.GetMessagesForUser(
                userId, currentUserId, messageParams);

            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetSendMessages_ShouldReturnCorrentNumberOfMessages()
        {
            var currentUserId = Guid.NewGuid();

            var messages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test message",
                    Sender = new User() {Id = currentUserId, Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() {Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                }
            };

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var pagedMessages = PagedList<Message>.ToPagedList(
                messages, messageParams.PageNumber, messageParams.PageSize);

            _unitOfWorkMock.Setup(x => x.Message.GetSentMessagesAsync(
                currentUserId, messageParams))
                .ReturnsAsync(pagedMessages);

            var response = await _messageSut.GetSentMessages(currentUserId, messageParams);

            Assert.True(response.Success);
            Assert.Equal(messages.Count(), response.Content.Count());
        }

        [Fact]
        public async Task GetSendMessages_ShouldReturn_UnsuccessfullResnpose_WhenUserDoesntHaveAnySendMessages()
        {
            var currentUserId = Guid.NewGuid();

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(x => x.Message.GetSentMessagesAsync(
                currentUserId, messageParams))
                .ReturnsAsync(() => null);

            var response = await _messageSut.GetSentMessages(currentUserId, messageParams);

            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetSendMessagesForUser_ShouldReturnCorrentNumberOfMessages()
        {
            var currentUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var messages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test message",
                    Sender = new User() {Id = currentUserId, Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() {Id = userId, Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                }
            };

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var pagedMessages = PagedList<Message>.ToPagedList(
                messages, messageParams.PageNumber, messageParams.PageSize);

            _unitOfWorkMock.Setup(x => x.Message.GetSentMessagesForUserAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(pagedMessages);

            var response = await _messageSut.GetSentMessagesForUser(
                currentUserId, userId, messageParams);

            Assert.True(response.Success);
            Assert.Equal(messages.Count(), response.Content.Count());
        }

        [Fact]
        public async Task GetSendMessagesForUser_ShouldReturn_UnsuccessfullResnpose_WhenUserDoesntHaveAnySendMessagesForUser()
        {
            var currentUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(x => x.Message.GetSentMessagesForUserAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(() => null);

            var response = await _messageSut.GetSentMessagesForUser(
                currentUserId, userId, messageParams);

            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetConverstion_ShouldReturnCorrectNumberOfMessages()
        {
            var currentUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var messages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test message",
                    Sender = new User() {Id = currentUserId, Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() {Id = userId, Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                },
                new Message()
                {
                    Content = "Test message 2",
                    Sender = new User() {Id = userId, Name = "senderTest", Email = "senderEmail@test.com" },
                    SenderUsername = "senderTest",
                    Recipient = new User() {Id = currentUserId, Name = "recipientTest", Email = "recipientEmail@test.com" },
                    RecipientUsername = "recipientTest",
                },
            };

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var pagedMessages = PagedList<Message>.ToPagedList(
                messages, messageParams.PageNumber, messageParams.PageSize);

            _unitOfWorkMock.Setup(x => x.Message.GetConversationAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(pagedMessages);

            var response = await _messageSut.GetConversation(
                currentUserId, userId, messageParams);

            Assert.True(response.Success);
            Assert.Equal(messages.Count(), response.Content.Count());
        }

        [Fact]
        public async Task GetConversation_ShouldReturn_UnsuccessfullResnpose_WhenUserDoesntHaveAnyConversation()
        {
            var currentUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var messageParams = new MessageParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(x => x.Message.GetConversationAsync(
                userId, currentUserId, messageParams))
                .ReturnsAsync(() => null);

            var response = await _messageSut.GetConversation(
                currentUserId, userId, messageParams);

            Assert.False(response.Success);
        }
    }
}
