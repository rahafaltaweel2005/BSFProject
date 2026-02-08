using Application.Generic_DTOs;
using Application.Services.ChatService.DTOs;

namespace Application.Services.ChatService
{
    public interface IChatService
    {
        Task SendChatMessage(SendChatMessageRequest request);
        Task<PaginationResponse<GetChatResponse>> GetUserChats(PaginationRequest request);
        Task<PaginationResponse<GetChatMessageResponse>> GetMessagesByChatId(PaginationRequest request,int chatId);
        Task UpdateMessageIsRead(int messageId);
    }
}