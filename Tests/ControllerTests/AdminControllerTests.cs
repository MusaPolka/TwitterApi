using DomainLayer.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public class AdminControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _testClient;
        public AdminControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _testClient = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(FakePolicyEvaluator));

                    services.Remove(descriptor);
                    services.AddSingleton<IPolicyEvaluator, AdminFakePolicyEvaluator>();

                    var serviceProvider = services.BuildServiceProvider();
                });
                
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturn_OkStatusCode()
        {
            var response = await _testClient.GetAsync("api/admin/GetAllUsers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        public async Task BanUser_ShouldReturn_OkStatusCode(string userName)
        {
            var response = await _testClient.PutAsJsonAsync($"api/admin/BanUser/{userName}", userName);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        public async Task UnbanUser_ShouldReturn_OkStatusCode(string userName)
        {
            var response = await _testClient.PutAsJsonAsync($"api/admin/UnbanUser/{userName}", userName);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
