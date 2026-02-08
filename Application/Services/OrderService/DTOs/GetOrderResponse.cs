namespace Application.Services.OrderService.DTOs
{
    public class GetOrderResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceProviderName { get; set; }
        public string ClientUserName { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}