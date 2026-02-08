namespace Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public int FirstUserId { get; set; }
        public User FirstUser { get; set; }
        public int SecondUserId { get; set; }
        public User SecondUser { get; set; }
        public DateTime LastMessageDate { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }

       
    }
}