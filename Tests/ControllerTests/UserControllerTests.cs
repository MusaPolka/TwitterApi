using DomainLayer.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tests.DataSeeders;
using Tests.Fakes;
using Xunit;

namespace Tests.ControllerTests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _testClient;
        public UserControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _testClient = _factory.CreateClient();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        public async Task GetUserByName_ShouldReturn_OkStatusCode(string userName)
        {
            var response = await _testClient.GetAsync($"/api/user/GetUserByName/{userName}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("test789")]
        [InlineData("___")]
        public async Task GetUserByName_ShouldReturn_NotFound_WhenUserName_is_Incorrect(string userName)
        {
            var response = await _testClient.GetAsync($"/api/user/GetUserByName/{userName}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test2", "test2")]
        public async Task GetUserByName_ShouldReturn_CorrectData(string userName, string expectedName)
        {
            var response = await _testClient.GetAsync($"/api/user/GetUserByName/{userName}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<User>();

            data.Should().BeOfType<User>();
            data.Should().NotBeNull();
            data.Name.Should().Be(expectedName);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturn_OkStatusCode()
        {
            var response = await _testClient.GetAsync($"/api/user/GetCurrentUser");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturn_CorrectUser()
        {
            var response = await _testClient.GetAsync($"/api/user/GetCurrentUser");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<User>();

            data.Should().BeOfType<User>();
            data.Should().NotBeNull();
            data.Email.Should().Be("test@gmail.com");
            data.Name.Should().Be("test");
        }     
    }
}
