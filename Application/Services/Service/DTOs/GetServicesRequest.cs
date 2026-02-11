using Application.Generic_DTOs;

namespace Application.Services.Service.DTOs
{
    public class GetServicesRequest:PaginationRequest
    {
           public int CategoryId  { get; set; }
           public string? Name{ get; set; }
    }
}