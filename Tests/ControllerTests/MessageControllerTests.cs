using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using ServiceLayer.Contracts;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tests.DataSeeders;
using Tests.Fakes;
using TwitterAPI.Controllers;
using Xunit;

namespace Tests.ControllerTests
{
    public class MessageControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _testClient;
        public MessageControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _testClient = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<ApplicationDbContext>();


                        DataGeneratorTests.ReInitialize(db);
                    }
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [Fact]
        public async Task GetAllReceivedMessages_ShouldReturn_OkStatusCode()
        {
            var response = await _testClient.GetAsync("/api/user/message/GetAllReceivedMessages");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllReceivedMessages_ShouldReturn_AnyData()
        {
            var response = await _testClient.GetAsync("/api/user/message/GetAllReceivedMessages");

            var data = await response.Content.ReadAsAsync<List<Message>>();

            data.Should().HaveCount(1);
        }


        [Theory]
        [InlineData("test", "test2", "test message")]
        public async Task GetMessagesForUser_ShouldReturn_CorrectData(string receiverName, 
            string senderName, string content)
        {
            var expectedlReceiverName = receiverName;
            var expectedSenderName = senderName;
            var expectedContent = content;

            var response = await _testClient.GetAsync($"/api/user/message/GetReceivedMessagesForUser/{senderName}");

            var data = await response.Content.ReadAsAsync<List<Message>>();

            Assert.Equal(expectedContent, data[0].Content);
            Assert.Equal(expectedSenderName, data[0].SenderUsername);
            Assert.Equal(expectedlReceiverName, data[0].RecipientUsername);
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldReturn_OkStatusCode()
        {
            var response = await _testClient.GetAsync("/api/user/message/GetReceivedMessagesForUser/test");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("IncorrectName")]
        [InlineData("IncorrectName2")]
        [InlineData("IncorrectName3")]
        public async Task GetMessagesForUser_ShouldReturn_NotFound_WhenReceiverName_Is_Incorrect(
            string incorrectName)
        {
            var response = await _testClient.GetAsync($"/api/user/message/GetMessagesForUser/{incorrectName}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetConversation_ShouldReturn_OkStatusCode()
        {
            var response = await _testClient.GetAsync("/api/user/message/GetConversation/test");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SendMessage_ShouldReturn_OkStatusCode()
        {
            var createMessageDto = new CreateMessageDto()
            {
                Message = "SendMessage test message",
                ResipientName = "test2"
            };

            var response = await _testClient.PostAsJsonAsync(
                "/api/user/message/SendMessage", createMessageDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
