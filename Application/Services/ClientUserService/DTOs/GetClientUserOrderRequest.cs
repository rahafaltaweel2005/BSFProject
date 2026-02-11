using Application.Generic_DTOs;

namespace Application.Services.ClientUserService.DTOs
{
    public class GetClientUserOrderRequest : PaginationRequest
    {
        public string? ServiceName { get; set; }
        public string? ServiceProviderName { get; set; }
    }
}