using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using ServiceLayer.Services;
using DomainLayer.DTOs;
using System.Security.Claims;
using ServiceLayer.Contracts;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        public TweetController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Tweet([FromBody] TweetDto tweetDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Not a valid model!");
            }
            if (tweetDto.Content == null && tweetDto.AttachedFile == null && ModelState.IsValid)
            {
                return BadRequest("Not a valid model!");
            }
            var email = HttpContext.User.FindFirstValue("emails");

            await _tweetService.CreateTweetAsync(tweetDto, email);

            return Ok();
        }
        [HttpGet("GetTweetsByUserName/{userName}")]
        public async Task<IActionResult> GetTweetsByUserName(string userName)
        {
            var tweetsByUserName = await _tweetService.GetTweetsByUserNameAsync(userName);

            return Ok(tweetsByUserName);
        }
    }
}
