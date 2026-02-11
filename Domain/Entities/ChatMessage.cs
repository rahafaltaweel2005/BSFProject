namespace Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReciverId { get; set; }
        public User Reciver { get; set; }   
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
    }
}