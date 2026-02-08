using Application.Sarvices.AuthService;
using Application.Services.AuthService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task <IActionResult> Login(LoginRequest request)
        {
            var result= await _authService.Login(request);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return Ok(result);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RrefreshToken(string refreshToken)
        {
            var newAccessToken = await _authService.GenerateNewAccessToken(refreshToken);
            if(newAccessToken == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }
            return Ok(newAccessToken);
        }
        [HttpPost("ChangeMyPassword")]
        public async Task<IActionResult> ChangeMyPassword([FromBody] ChangeMyPasswordRequest request)
        {
            await _authService.ChangeMyPasswoerd(request);
            return Ok();
        }

    }
}