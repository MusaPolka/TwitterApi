using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using ServiceLayer.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Authenticate")]
        public IActionResult Authenticate()
        {
            var url = _authService.CreateAuthUrl();

            return Redirect(url);
        }

        [HttpGet]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            var token = HttpContext.Request.Query["access_token"].ToString();

            if (token == null) return NotFound();

            await _authService.RegisterUser(token);

            //if (user == null) return Ok(token);

            return Ok(token);
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            var url = _authService.CreateLogoutUrl();

            return Redirect(url);
        }
        
    }
}

