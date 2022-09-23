using DomainLayer.DTOs;
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
    public class CommentController : ControllerBase
    {
        private readonly IUserInteractionService _userInteractionService;
        public CommentController(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CommentTweet(CommentDto commentDto)
        {
            var email = HttpContext.User.FindFirstValue("emails");

            await _userInteractionService.CommentTweet(commentDto, email);

            return Ok();
        }
    }
}
