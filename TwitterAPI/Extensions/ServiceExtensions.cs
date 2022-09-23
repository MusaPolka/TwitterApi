using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Implementations;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using ServiceLayer.Services;

namespace TwitterAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("Azure"));
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserInteractionService, UserInteractionService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddScoped<ITweetService, TweetService>();
        }
    }
}
