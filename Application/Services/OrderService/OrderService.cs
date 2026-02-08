using Application.Repositories;
using Application.Services.OrderService.DTOs;
using Application.Services.CurrentUserService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Application.Generic_DTOs;

namespace Application.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Domain.Entities.Service> _serviceRepo;
                private readonly IGenericRepository<ClientUser> _clientUserRepo;

        private readonly ICurrentUserService _currentUserService;
        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<Domain.Entities.Service> serviceRepo, ICurrentUserService currentUserService,IGenericRepository<ClientUser> clientUserRepo)
        {
            _orderRepo = orderRepo;
            _serviceRepo = serviceRepo;
            _currentUserService = currentUserService;
            _clientUserRepo=clientUserRepo;
        }
        public async Task<PaginationResponse<GetOrderResponse>> GetClientUserOrders(PaginationRequest request)
        {
            var query = _orderRepo.GetAll().Where(x => x.ClientUserId == _currentUserService.ClientUserId)
            .OrderByDescending(x=>x.CreatedTime);
            var count = await query.CountAsync();
            var result = await query .Skip(request.PageSize * request.PageIndex).Take(request.PageSize)
            .Include(x => x.Service)
            .ThenInclude(x => x.ServiceProvider).ThenInclude(x => x.User).Select(x => new GetOrderResponse
            {
                Id = x.Id,
                ServiceName = x.Service.Name,
                ServiceProviderName = x.Service.ServiceProvider.User.Name,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                Note = x.Note,
                Status = x.Status.ToString(),
                CreatedTime = x.CreatedTime
            }).ToListAsync();
            return new PaginationResponse<GetOrderResponse>
            {
                Item = result,
                Count = count
            };
        }
         public async Task<PaginationResponse<GetOrderResponse>> GetServiceProviderOrders(PaginationRequest request)
        {
            var query = _orderRepo.GetAll().Where(x => x.ServiceProviderId == _currentUserService.ServiceProviderId)
            .OrderByDescending(x=>x.CreatedTime);
            var count =await query.CountAsync();
            var result = await query.Skip(request.PageSize * request.PageIndex).Take(request.PageSize).Include(x => x.Service)
            .ThenInclude(x => x.ServiceProvider).ThenInclude(x => x.User).Select(x => new GetOrderResponse
            {
                Id = x.Id,
                ServiceName = x.Service.Name,
                ServiceProviderName = x.Service.ServiceProvider.User.Name,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                Note = x.Note,
                Status = x.Status.ToString(),
                CreatedTime = x.CreatedTime
            }).ToListAsync();
            return new PaginationResponse<GetOrderResponse>
            {
                Item = result,
                Count = count
            };
        }

        public async Task OrderRequest(SaveOrderRequest request)
        {
            var service = await _serviceRepo.GetAll().Include(s => s.ServiceProvider).FirstOrDefaultAsync(s => s.Id == request.ServiceId);
            if (!service.ServiceProvider.IsAvailable)
            {
                throw new Exception("Service provider is not available");
            }
            if (request.FromTime >= request.ToTime)
            {
                throw new Exception("FromTime must be less than ToTime");
            }
            var isAnyOrderExistinSameTime = await _orderRepo.GetAll().AnyAsync(
                o => o.ServiceId == request.ServiceId &&
                o.FromTime.TimeOfDay == request.FromTime.TimeOfDay &&
                o.ToTime.TimeOfDay == request.ToTime.TimeOfDay
            );
            if (isAnyOrderExistinSameTime)
            {
                throw new Exception("Service provider is not available");
            }

            var order = new Order
            {
                ServiceId = request.ServiceId,
                ServiceProviderId = service.ServiceProviderId,
                ClientUserId = _currentUserService.ClientUserId.Value,
                FromTime = request.FromTime,
                ToTime = request.ToTime,
                Note = request.Note,
                Status = OrderStatus.Pending,
                CreatedTime = DateTime.UtcNow
            };
            await _orderRepo.InsertAsync(order);
            await _orderRepo.SaveChangesAsync();
        }

        public async Task UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            var order = await _orderRepo.GetBYIdAsync(request.orderId);
            order.Status = request.Status;
            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();

        }
        public async Task DeleteOrder(int orderId)
        {
            var order = await _orderRepo.GetBYIdAsync(orderId);
            if (order.Status == OrderStatus.Pending)
            {
                throw new Exception("Can't delete order");
            }
            _orderRepo.Delete(order);
            await _orderRepo.SaveChangesAsync();
        }

    }
}