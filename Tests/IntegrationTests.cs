using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tests.DataSeeders;
using Tests.Fakes;
using TwitterAPI.Extensions;
using Xunit;

namespace Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _testClient;
        public IntegrationTests()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(webHostBuilder =>
                {
                    webHostBuilder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestsInMemory");
                        });
                       

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<ApplicationDbContext>();


                            db.Database.EnsureCreated();

                            //DataGeneratorTests.Initialize(db);
                        }
                    });
                }); 

            _testClient = factory.CreateClient();
        }
    }
}
