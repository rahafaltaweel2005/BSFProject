namespace Domain.Entities
{
    public class FirebaseToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}