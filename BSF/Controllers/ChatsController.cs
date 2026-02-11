using Application.Generic_DTOs;
using Application.Services.ChatService;
using Application.Services.ChatService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("SendChatMessage")]
        public async Task<IActionResult> SendChatMessage([FromBody] SendChatMessageRequest request)
        {
            await _chatService.SendChatMessage(request);
            return Ok();
        }

        [HttpPost("GetUserChats")]
        public async Task<IActionResult> GetUserChats(PaginationRequest request)
        {
            var response = await _chatService.GetUserChats(request);
            return Ok(response);
        }

        [HttpPost("GetMessagesByChatId")]
        public async Task<IActionResult> GetMessagesByChatId(PaginationRequest request,int chatId)
        {
            var response = await _chatService.GetMessagesByChatId(request,chatId);
            return Ok(response);
        }

        [HttpPost("UpdateMessageIsRead")]
        public async Task<IActionResult> UpdateMessageIsRead(int messageId)
        {
            await _chatService.UpdateMessageIsRead(messageId);
            return Ok();
        }


    }
}