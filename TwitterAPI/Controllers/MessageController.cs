using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System.Security.Claims;

namespace TwitterAPI.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, IUserService userService,
            IMapper mapper)
        {
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAllReceivedMessages")]
        public async Task<IActionResult> GetAllReceivedMessages([FromQuery] MessageParams messageParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var response = await _messageService.GetMessages(currentUser.Id, messageParams);

            if(!response.Success) return BadRequest(response.Message);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Content.Metadata));

            var messages = _mapper.Map<IEnumerable<MessageDto>>(response.Content);

            return Ok(messages);
        }

        [HttpGet("GetAllSentMessages")]
        public async Task<IActionResult> GetAllSentMessages([FromQuery]MessageParams messageParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var response = await _messageService.GetSentMessages(currentUser.Id, messageParams);

            if (!response.Success) return BadRequest(response.Message);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Content.Metadata));

            var messages = _mapper.Map<IEnumerable<MessageDto>>(response.Content);

            return Ok(messages);
        }

        [HttpGet("GetSentMessagesForUser/{userName}")]
        public async Task<IActionResult> GetSentMessagesForUser(string userName, [FromQuery]MessageParams messageParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return BadRequest("User is not found");

            var receiver = await _userService.GetUserByNameAsync(userName);

            if (receiver == null) return NotFound("User is not found");

            var response = await _messageService.GetSentMessagesForUser(currentUser.Id, receiver.Id, messageParams);

            if (!response.Success) return BadRequest(response.Message);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Content.Metadata));

            var messages = _mapper.Map<IEnumerable<MessageDto>>(response.Content);

            return Ok(messages);
        }

        [HttpGet("GetReceivedMessagesForUser/{userName}")]
        public async Task<IActionResult> GetReceivedMessagesForUser(string userName, [FromQuery] MessageParams messageParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("User is not found");

            var receiver = await _userService.GetUserByNameAsync(userName);

            if (receiver == null) return NotFound("User is not found");

            var response = await _messageService.GetMessagesForUser(receiver.Id, currentUser.Id, messageParams);

            if (!response.Success) return NotFound(response.Message);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Content.Metadata));

            var resultMessage = _mapper.Map<IEnumerable<MessageDto>>(response.Content);

            return Ok(resultMessage);
        }

        [HttpGet("GetConversation/{receiverName}")]
        public async Task<IActionResult> GetConversation(string receiverName, [FromQuery] MessageParams messageParams)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("User is not found");

            var receiver = await _userService.GetUserByNameAsync(receiverName);

            if (receiver == null) return NotFound("User is not found");

            var response = await _messageService.GetConversation(currentUser.Id, receiver.Id, messageParams);

            if (!response.Success) return NotFound(response.Message);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Content.Metadata));

            var resultMessage = _mapper.Map<IEnumerable<MessageDto>>(response.Content);

            return Ok(resultMessage);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody]CreateMessageDto createMessageDto)
        {
            var name = HttpContext.User.FindFirstValue("name");

            var currentUser = await _userService.GetCurrentUserAsync(name);

            if (currentUser == null) return NotFound("User is not found");

            var receiver = await _userService.GetUserByNameAsync(createMessageDto.ResipientName);

            if(receiver == null) return NotFound("User is not found");

            var message = new Message()
            {
                SenderId = currentUser.Id,
                Sender = currentUser,
                RecipientId = receiver.Id,
                Recipient = receiver,
                SenderUsername = currentUser.Name,
                RecipientUsername = receiver.Name,
                Content = createMessageDto.Message
            };

            var response = await _messageService.SendMessage(message);

            if (!response.Success) return BadRequest(response.Message);

            var resultMessage = _mapper.Map<MessageDto>(message);

            return Ok(resultMessage);
        }
    }
}
