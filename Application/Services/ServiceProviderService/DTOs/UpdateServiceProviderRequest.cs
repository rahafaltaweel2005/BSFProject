using Microsoft.AspNetCore.Http;

namespace Application.Services.ServiceProviderService.DTOs
{
    public class UpdateServiceProviderRequest : ServiceProviderRegistrationRequest
    {
        public bool IsAvailable { get; set; }
        public IFormFile? PersonalPhoto { get; set; }
        public bool DeletePhoto { get; set; }
    }
}