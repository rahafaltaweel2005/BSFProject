using Microsoft.AspNetCore.Http;

namespace Application.Services.ClientUserService.DTOs
{
    public class UpdateClientUserRequest : ClientUserRegistrationRequest
    {
        public IFormFile? PersonalPhoto { get; set; }
        public bool DeletePhoto { get; set; }
    }
}