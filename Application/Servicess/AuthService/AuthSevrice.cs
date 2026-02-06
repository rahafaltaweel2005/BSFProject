using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Repositories;
using Application.Servicess.AuthService.DTOs;
using Application.Servicess.CurrentUserService;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace Application.Sarvices.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<RefershToken> _refershTokenRepository;
        public AuthService(IGenericRepository<User> userRepository, IGenericRepository<RefershToken> refershTokenRepository, IConfiguration config , ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _refershTokenRepository = refershTokenRepository;
            _config = config;
            _currentUserService = currentUserService;
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Username.Trim().ToLower() || u.PhoneNumber == request.Username.Trim());
            if (user == null)
            {
                return null;
            }
            var passwordHasher = new PasswordHasher<User>();
            var PasswordRequest = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (PasswordRequest == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var refershToken = new RefershToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };
            await _refershTokenRepository.InsertAsync(refershToken);
            await _refershTokenRepository.SaveChangesAsync();
            var result = new LoginResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleCode = user.Role.Code,
                AccessToken = await GenerateAccessToken(user),
                RefreshToken = refershToken.Token,
            };
            return result;
        }
        public async Task<string> GenerateNewAccessToken(string refershToken)
        {
            var token = await _refershTokenRepository.GetAll().FirstOrDefaultAsync(t => t.Token == refershToken);
            if (token == null || token.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }
            var user = await _userRepository.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == token.UserId);
            return await GenerateAccessToken(user);
        }
        private async Task<string> GenerateAccessToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }


        private string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }

        public async Task ChangeMyPasswoerd(ChangeMyPasswordRequest request)
        {
            var  user = await _userRepository.GetBYIdAsync(_currentUserService.UserId.Value);
            var passwordHasher=new PasswordHasher<User>();
            var PasswordResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.CurrentPassword);
            if (PasswordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Current password is incorrect");
            }
            if(request.NewPassword != request.ConfirmNewPassword)
            {
                throw new ArgumentException("New password and confirmation do not match");
            }
            user.Password = passwordHasher.HashPassword(user, request.NewPassword);
        }
    }
}