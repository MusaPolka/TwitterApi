using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;
using ServiceLayer.Services;
using System.Security.Claims;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IUserInteractionService _userInteractionService;
        public LikeController(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikeTweet(Guid tweetId)
        {
            var email = HttpContext.User.FindFirstValue("emails");

            await _userInteractionService.LikeTweet(tweetId, email);

            return Ok();
        }
    }
}
