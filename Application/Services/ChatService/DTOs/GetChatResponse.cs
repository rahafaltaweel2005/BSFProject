namespace Application.Services.ChatService.DTOs
{
    public class GetChatResponse
    {
        public int Id { get; set; }
        public int SecondUserId { get; set; }
            public int FirstUserId { get; set; }
        public string FirstUserName { get; set; }
        public string SecondUserName { get; set; }
        public string LastMessage { get; set; }



    }
}