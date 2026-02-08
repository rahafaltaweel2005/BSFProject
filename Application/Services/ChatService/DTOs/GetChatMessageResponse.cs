namespace Application.Services.ChatService.DTOs
{
    public class GetChatMessageResponse
    {
        public int Id { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public string Message { get; set; }

    }
}