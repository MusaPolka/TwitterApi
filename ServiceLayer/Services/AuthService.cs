using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork; 

        public AuthService(IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public string CreateAuthUrl()
        {
            var instance = _configuration["AzureAdB2C:Instance"];

            var domain = _configuration["AzureAdB2C:Domain"];

            var policy = _configuration["AzureAdB2C:SignUpSignInPolicyId"];

            var clientId = _configuration["AzureAdB2C:ClientId"];

            var redirectUri = "https://localhost:7153/api/auth/gettoken";

            var scope = $"openid+offline_access+https://{domain}/{clientId}/Files.Read";

            var url = $"{instance}/{domain}/{policy}/oauth2/v2.0/authorize?client_id={clientId}&response_type=token&redirect_uri={redirectUri}&response_mode=query&scope={scope}";

            return url;
        }

        public string CreateLogoutUrl()
        {
            var instance = _configuration["AzureAdB2C:Instance"];

            var domain = _configuration["AzureAdB2C:Domain"];

            var policy = _configuration["AzureAdB2C:SignUpSignInPolicyId"];

            var redirectUri = "https://localhost:7153";

            var url = $"{instance}/{domain}/{policy}/oauth2/v2.0/logout?post_logout_redirect_uri={redirectUri}";

            return url;
        }

        public async Task<User> RegisterUser(string token)
        {
            var claims = GetClaimsFromToken(token);

            var emailClaim = claims.FirstOrDefault(x => x.Type == "email");

            if(emailClaim == null) return null;

            var email = emailClaim.Value;

            if (await UserExists(email)) return null;

            var user = new User
            {
                Name = claims.FirstOrDefault(x => x.Type == "name").Value,
                Email = email,
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
                AccountType = Accessibility.Public,
                Status = AccountStatus.Active
            };

            _unitOfWork.User.Add(user);

            await _unitOfWork.SaveAsync();

            return user;
        }

        public IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

            return securityToken.Claims;
        }

        public async Task<bool> UserExists(string name)
        {
            return await _unitOfWork.User.UserExistsAsync(name);
        }
    }
}
