using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IAuthService
    {
        string CreateAuthUrl();
        string CreateLogoutUrl();
        Task<User> RegisterUser(string token);
        Task<bool> UserExists(string name);
        IEnumerable<Claim> GetClaimsFromToken(string token);
    }
}
