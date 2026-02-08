namespace Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
         public int FirstUserId { get; set; }
        public User FirstUser { get; set; }
        public int SecondUserId { get; set; }
        public User SecondUser { get; set; }        
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
    }
}