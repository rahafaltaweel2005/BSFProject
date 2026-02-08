namespace Domain.Entities
{
    public class Notification
    {
         public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedData { get; set; }
        public bool IsRead{ get; set; }
    }
}