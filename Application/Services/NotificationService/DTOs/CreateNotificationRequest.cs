namespace Application.Services.NotificationService.DTOs
{
    public class CreateNotificationRequest
    {

        public int UserId { get; set; }
        public int? OrderId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedData { get; set; }
        public bool IsRead { get; set; }


    }
}