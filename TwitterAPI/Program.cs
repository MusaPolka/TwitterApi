using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using ServiceLayer.Services;
using TwitterAPI.Extensions;
using TwitterAPI.Middlewares;

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
{
    string databaseName = configurationSection.GetSection("DatabaseName").Value;
    string containerName = configurationSection.GetSection("ContainerName").Value;
    string account = configurationSection.GetSection("Account").Value;
    string key = configurationSection.GetSection("Key").Value;
    Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

    return cosmosDbService;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.ConfigureSwaggerGen();

builder.Services.ConfigureCors();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlob")));

builder.Host.ConfigureAppConfiguration(options =>
{
    options.AddAzureKeyVault(new SecretClient
    (
        new Uri(builder.Configuration["KeyVaultConfigs:KeyVaultURL"]),
        new ClientSecretCredential
        (
            builder.Configuration["KeyVaultConfigs:TenantId"],
            builder.Configuration["KeyVaultConfigs:ClientId"],
            builder.Configuration["KeyVaultConfigs:ClientSecretId"]
        )
    ), new AzureKeyVaultConfigurationOptions());
});


builder.Services.ConfigureServices();


builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, Constants.AzureAdB2C)
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim(ClaimConstants.Role, "admin"));
        //.RequireAssertion(context =>
            //context.User.HasClaim(c => c.Type == "role" && c.Value == "admin")));
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
      options.SerializerSettings.ReferenceLoopHandling =
        ReferenceLoopHandling.Ignore);


builder.Host.ConfigureAppConfiguration(options =>
{
    options.AddAzureKeyVault(new SecretClient
    (
        new Uri(builder.Configuration["KeyVaultConfigs:KeyVaultURL"]),
        new ClientSecretCredential
        (
            builder.Configuration["KeyVaultConfigs:TenantId"],
            builder.Configuration["KeyVaultConfigs:ClientId"],
            builder.Configuration["KeyVaultConfigs:ClientSecretId"]
        )
    ), new AzureKeyVaultConfigurationOptions());
});


builder.Services.AddOptions();



builder.Services.Configure<OpenIdConnectOptions>(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
builder.Services.ConfigureServices();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterAPI v1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionMiddleware>();


app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<BannedUserMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();

public partial class Program { }

