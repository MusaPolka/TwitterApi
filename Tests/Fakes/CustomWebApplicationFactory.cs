using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DataSeeders;

namespace Tests.Fakes
{
    public class CustomWebApplicationFactory<TProgram> 
        : WebApplicationFactory<TProgram> where TProgram: class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                });

                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                

                    db.Database.EnsureCreated();

                    try
                    {
                        DataGeneratorTests.ReInitialize(db);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }
    }
}
