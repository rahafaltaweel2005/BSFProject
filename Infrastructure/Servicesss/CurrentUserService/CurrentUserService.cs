using Application.Servicess.CurrentUserService;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Servicesss.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? UserId
        {
            get
            {
               return Convert.ToInt32(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);   
            }
        }

        public string? Name
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            }
        }

        public string? Email{
            get
            {
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            }
        }

        public string? MobilePhone {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "phone")?.Value;
            }
        }

        public String? Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            }
        }
    }
}