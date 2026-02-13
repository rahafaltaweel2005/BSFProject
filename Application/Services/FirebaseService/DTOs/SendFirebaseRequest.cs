namespace Application.Services.FirebaseService.DTOs
{
    public class SendFirebaseRequest
    {
        public List<string> Tokens { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}