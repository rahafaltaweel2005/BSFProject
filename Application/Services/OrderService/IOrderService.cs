using Application.Generic_DTOs;
using Application.Services.OrderService.DTOs;

namespace Application.Services.OrderService
{
    public interface IOrderService
    {
        Task OrderRequest(SaveOrderRequest request);
        Task<PaginationResponse<GetOrderResponse>> GetClientUserOrders(PaginationRequest request);
        Task<PaginationResponse<GetOrderResponse>> GetServiceProviderOrders(PaginationRequest request);
        Task UpdateOrderStatus(UpdateOrderStatusRequest request);
        Task DeleteOrder(int orderId);

    }
}