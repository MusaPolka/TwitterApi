using DomainLayer.DTOs;
using DomainLayer.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;
using System.Security.Claims;

namespace TwitterAPI.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IUserInteractionService _userInteractionService;
        private readonly IUserService _userService;

        public FollowController(IUserInteractionService userInteractionService, IUserService userService)
        {
            _userInteractionService = userInteractionService;
            _userService = userService;
        }

        [HttpPost("FollowToUser/{userToFollowName}")]
        public async Task<IActionResult> FollowToUser(string userToFollowName)
        {
            if (userToFollowName == null) return BadRequest("userToFollowName is null");

            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("User is not found");

            var userToFollow = await _userService.GetUserByNameAsync(userToFollowName);

            if (userToFollow == null) return NotFound($"{userToFollowName} is not found");

            var requestToFollow = new RequestToFollow()
            {
                SourceUserId = currentUser.Id,
                SourceUser = currentUser,
                FollowedUserId = userToFollow.Id,
                FollowedUser = userToFollow
            };

            var response = await _userInteractionService.Follow(requestToFollow);

            if (!response.Success) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpPost("AcceptFollowRequest/{userToAcceptName}")]
        public async Task<IActionResult> AcceptFollowRequest(string userToAcceptName)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var userToAccept = await _userService.GetUserByNameAsync(userToAcceptName);

            if (userToAccept == null) return BadRequest($"{userToAcceptName} is not found");

            var requestToFollow = new RequestToFollow()
            {
                SourceUserId = userToAccept.Id,
                SourceUser = userToAccept,
                FollowedUserId = currentUser.Id,
                FollowedUser = currentUser
            };

            var response = await _userInteractionService.AcceptRequestToFollow(requestToFollow);

            if (!response.Success) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpPost("RefuseFollowRequest/{userToRefuseName}")]
        public async Task<IActionResult> RefuseFollowRequest(string userToRefuseName)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var userToRefuse = await _userService.GetUserByNameAsync(userToRefuseName);

            if (userToRefuse == null) return BadRequest($"{userToRefuseName} is not found");

            var requestToFollow = new RequestToFollow()
            {
                SourceUserId = userToRefuse.Id,
                SourceUser = userToRefuse,
                FollowedUserId = currentUser.Id,
                FollowedUser = currentUser
            };

            var response = await _userInteractionService.RefuseRequestToFollow(requestToFollow);

            if (!response.Success) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpDelete("UnfollowUser/{userToUnfollowName}")]
        public async Task<IActionResult> UnfollowUser(string userToUnfollowName)
        {
            if (userToUnfollowName == null) return BadRequest("userToUnfollowName is null");

            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var userToUnfollow = await _userService.GetUserByNameAsync(userToUnfollowName);

            if (userToUnfollow == null) return BadRequest($"{userToUnfollow} is not found");

            var requestToUnfollow = new RequestToUnfollow()
            {
                SourceUserId = currentUser.Id,
                SourceUser = currentUser,
                UnfollowedUserId = userToUnfollow.Id,
                UnfollowedUser = userToUnfollow
            };

            var response = await _userInteractionService.Unfollow(requestToUnfollow);

            if (!response.Success) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("GetFollowRequests")]
        public async Task<IActionResult> GetFollowRequests([FromQuery] FollowParams followParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var requests = await _userInteractionService.GetFollowRequests(
                currentUser.Id, followParams);

            if (!requests.Success) return BadRequest(requests.Message);

            return Ok(requests);
        }

        [HttpGet("GetFollowings")]
        public async Task<IActionResult> GetFollowings([FromQuery] FollowParams followParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var requests = await _userInteractionService.GetFollowings(
                currentUser.Id, followParams);

            if (!requests.Success) return BadRequest(requests.Message);

            return Ok(requests);
        }

        [HttpGet("GetFollowers")]
        public async Task<IActionResult> GetFollowers([FromQuery] FollowParams followParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var requests = await _userInteractionService.GetFollowers(
                currentUser.Id, followParams);

            if (!requests.Success) return BadRequest(requests.Message);

            return Ok(requests);
        }

        
    }
}
