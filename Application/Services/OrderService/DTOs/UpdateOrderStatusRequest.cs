using Domain.Enums;

namespace Application.Services.OrderService.DTOs
{
    public class UpdateOrderStatusRequest
    {
        public int orderId { get; set; }
        public OrderStatus Status { get; set; }

    }
}