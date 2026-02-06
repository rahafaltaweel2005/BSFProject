using Domain.Enums;

namespace Application.Servicess.AuthService.DTOs
{
    public class LoginResponse
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
         public SystemRole RoleCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}