namespace Application.Services.OrderService.DTOs
{
    public class SaveOrderRequest
    {
        public int ServiceId { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string? Note { get; set; }
    }
}