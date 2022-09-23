using DomainLayer.Enums;
using DomainLayer.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Contracts;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserParams userParams)
        {
            var users = await _userService.GetAllUsersAsync(userParams);

            if (users == null) return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(users.Metadata));

            return Ok(users);
        }

        [HttpPut("BanUser/{userName}")]
        public async Task<IActionResult> BanUser(string userName)
        {
            if (userName == null) return BadRequest("Enter user name to ban");

            var user = await _userService.GetUserByNameAsync(userName);

            if (user == null) return NotFound("User not found");

            user.Status = AccountStatus.Banned;

            await _userService.Save();

            return Ok(user);
        }

        [HttpPut("UnbanUser/{userName}")]
        public async Task<IActionResult> UnbanUser(string userName)
        {
            if (userName == null) return BadRequest("Enter user name to ban");

            var user = await _userService.GetUserByNameAsync(userName);

            if (user == null) return NotFound("User not found");

            user.Status = AccountStatus.Active;

            await _userService.Save();

            return Ok(user);
        } 
    }
}
