using Application.Servicess.AuthService.DTOs;

namespace Application.Sarvices.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<string> GenerateNewAccessToken(string refershToken);
        Task ChangeMyPasswoerd(ChangeMyPasswordRequest request);
    }
}