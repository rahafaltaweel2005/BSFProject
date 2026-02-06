using Domain.Enums;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public int ClientUserId { get; set; }
        public ClientUser ClientUser { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string? Note { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}