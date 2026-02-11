using Application.Generic_DTOs;
using Application.Hubs;
using Application.Managers.Chat;
using Application.Repositories;
using Application.Services.ChatService.DTOs;
using Application.Services.CurrentUserService;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ChatService
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<Chat> _chatRepo;
        private readonly IGenericRepository<ChatMessage> _chatMessageRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatConnectionManager _chatConnectionManager;

        public ChatService(IGenericRepository<Chat> chatRepo, IGenericRepository<ChatMessage> chatMessageRepo, ICurrentUserService currentUserService, IHubContext<ChatHub> hubContext, IChatConnectionManager chatConnectionManager)
        {
            _chatRepo = chatRepo;
            _chatMessageRepo = chatMessageRepo;
            _currentUserService = currentUserService;
            _hubContext=hubContext;
            _chatConnectionManager=chatConnectionManager;
        }

        public async Task SendChatMessage(SendChatMessageRequest request)
        {
            var UserId = _currentUserService.UserId.Value;
            var chat = new Chat();
            if (request.ChatId == null)
            {
                chat = new Chat
                {
                    FirstUserId = UserId,
                    SecondUserId = request.RecieverId,
                    LastMessageDate = DateTime.UtcNow
                };
                await _chatRepo.InsertAsync(chat);
                await _chatRepo.SaveChangesAsync();
            }
            else
            {
                chat = await _chatRepo.GetBYIdAsync(request.ChatId.Value);
                chat.LastMessageDate = DateTime.UtcNow;
                _chatRepo.Update(chat);
                await _chatRepo.SaveChangesAsync();
            }
            var ChatMessage = new ChatMessage
            {
                ChatId = chat.Id,
                SenderId = UserId,
                ReciverId = request.RecieverId,
                IsRead = false,
                CreatedDate = DateTime.UtcNow,
                Message = request.Message,
            };
            if(_chatConnectionManager.TryGet(ChatMessage.ReciverId.ToString(),out string connectionId))
            {
               await _hubContext.Clients.Client(connectionId).SendAsync("newMessage",ChatMessage);
            }
            await _chatMessageRepo.InsertAsync(ChatMessage);
            await _chatMessageRepo.SaveChangesAsync();
        }
        public async Task<PaginationResponse<GetChatResponse>> GetUserChats(PaginationRequest request)
        {
            var UserId = _currentUserService.UserId.Value;
            var query = _chatRepo.GetAll().Where(x => x.FirstUserId == UserId || x.SecondUserId == UserId)
            .OrderByDescending(x => x.LastMessageDate);
            var count = await query.CountAsync();
            var result = await query.Skip(request.PageSize * request.PageIndex).Take(request.PageSize).
             Include(x => x.SecondUser).Include(x => x.ChatMessages).Select(x => new GetChatResponse
             {
                 Id = x.Id,
                 LastMessage = x.ChatMessages.OrderByDescending(x => x.CreatedDate).FirstOrDefault().Message,
                FirstUserId = x.FirstUser.Id,
                    FirstUserName = x.FirstUser.Name,
                    SecondUserId = x.SecondUser.Id,
                    SecondUserName = x.SecondUser.Name,

             }).ToListAsync();
            return new PaginationResponse<GetChatResponse>
            {
                Item = result,
                Count = count
            };
        }

        public async Task<PaginationResponse<GetChatMessageResponse>> GetMessagesByChatId(PaginationRequest request, int chatId)
        {
            var query = _chatMessageRepo.GetAll().Where(x => x.ChatId == chatId).OrderByDescending(x => x.CreatedDate);
            var count = await query.CountAsync();
            var result = await query.Skip(request.PageSize * request.PageIndex).Take(request.PageSize)
            .Select(x => new GetChatMessageResponse
            {
                    Id = x.Id,
                    SenderId = x.SenderId,
                    ReciverId = x.ReciverId,
                    Message = x.Message,
                    IsRead = x.IsRead,

            }).ToListAsync();
            return new PaginationResponse<GetChatMessageResponse>
            {
                Item = result,
                Count = count
            };
        }

        public async Task UpdateMessageIsRead(int messageId)
        {
            var message = await _chatMessageRepo.GetBYIdAsync(messageId);
            message.IsRead = true;
            _chatMessageRepo.Update(message);
            await _chatMessageRepo.SaveChangesAsync();
        }
    }
}