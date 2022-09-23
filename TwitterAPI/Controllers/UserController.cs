using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Implementations;
using ServiceLayer.Contracts;
using System.Security.Claims;
using static TwitterAPI.Common.CustomValidation;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly UnitOfWork _unitOfWork;

        public UserController(IUserService userService, IMapper mapper, IBlobService blobService,
            UnitOfWork unitOfWork)
        {
            _userService = userService;
            _mapper = mapper;
            _blobService = blobService;
            _unitOfWork = unitOfWork;
        }

        

        [HttpGet("GetUserByName/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            if (userName == null) return NotFound();

            var user = await _userService.GetUserByNameAsync(userName);

            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var name = HttpContext.User.FindFirst("name");

            var currentUser = await _userService.GetCurrentUserAsync(name.Value);

            return Ok(currentUser);
        } 

        [HttpPatch("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] JsonPatchDocument<UpdateUserDto> updateUserDto)
        {
            if (updateUserDto == null) return BadRequest("Object is null");

            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("Current user not found");

            var userToUpdate = _mapper.Map<UpdateUserDto>(currentUser);

            updateUserDto.ApplyTo(userToUpdate);

            _mapper.Map(userToUpdate, currentUser);

            await _userService.Save();

            return Ok(userToUpdate);
        } 

        [HttpPut("UploadMainPhoto")]
        [Authorize]
        public async Task<IActionResult> UploadMainPhoto(
            [CheckFormat(new string[] {".jpg", ".jpeg"})]IFormFile formFile)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("Current user not found");

            using (var stream = formFile.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(stream, formFile.FileName, formFile.Headers.ContentType);
            }

            var url = _blobService.GetUri(formFile.FileName);

            currentUser.MainPhotoUrl = url;

            await _unitOfWork.SaveAsync();

            return Ok(currentUser);
        }

        [HttpDelete("DeleteMainPhoto")]
        [Authorize]
        public async Task<IActionResult> DeleteMainPhoto()
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("Current user not found");

            int pos = currentUser.MainPhotoUrl.LastIndexOf("/") + 1;

            var photoName = currentUser.MainPhotoUrl.Substring(pos, currentUser.MainPhotoUrl.Length - pos);

            await _blobService.DeleteBlobAsync(photoName);

            currentUser.MainPhotoUrl = null;

            await _unitOfWork.SaveAsync();
            
            return Ok(currentUser);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFollowingUserTweets()
        {
            var userName = HttpContext.User.FindFirstValue("username");

            var tweets = await _userService.GetFollowingUserTweetsAsync(userName);

            return Ok(tweets.Select(x => _mapper.Map<TweetDto>(x)));
        }

        [HttpPut("ChangeAccessibility")]
        public async Task<IActionResult> ChangeAccessibility(Accessibility accessibility)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("Current user not found");

            currentUser.AccountType = accessibility;

            await _unitOfWork.SaveAsync();

            return Ok(currentUser);
        }
    }
}
