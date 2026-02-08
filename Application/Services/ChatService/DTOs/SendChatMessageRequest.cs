namespace Application.Services.ChatService.DTOs
{
    public class SendChatMessageRequest
    {
        public int? ChatId { get; set; }
        public int RecieverId { get; set; }
        public string Message { get; set; }
    }
}