using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;
using System.Security.Claims;

namespace TwitterAPI.Controllers
{
    public class RetweetController : Controller
    {
        private readonly IUserInteractionService _userInteractionService;
        public RetweetController(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Retweet(Guid tweetId)
        {
            var email = HttpContext.User.FindFirstValue("emails");

            await _userInteractionService.Retweet(tweetId, email);

            return Ok();
        }
    }
}
