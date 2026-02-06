
using Application.Repositories;
using Application.Servicess.ClientUserService.DTOs;
using Application.Servicess.CurrentUserService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicess.ClientUserService
{
    public class ClientUserService : IClientUserService
    {
        private readonly IGenericRepository<ClientUser> _clientUserRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly ICurrentUserService _currentUserService;
        public ClientUserService(IGenericRepository<ClientUser> clientUserRepo, IGenericRepository<Role> roleRepo, IGenericRepository<User> userRepo, ICurrentUserService currentUserService)
        {
            _clientUserRepo = clientUserRepo;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _currentUserService = currentUserService;
        }
        public async Task ClientUserRegistration(ClientUserRegistrationRequest request)
        {
            await RegistrationValidation(request);
            var clientUserRole = await _roleRepo.GetAll().FirstOrDefaultAsync(x => x.Code == SystemRole.User);
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RoleId = clientUserRole.Id
            };
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, request.Password);
            await _userRepo.InsertAsync(user);
            await _userRepo.SaveChangesAsync();
            var ClientUser = new ClientUser
            {
                UserId = user.Id,
                BirthDate = request.BirthDate
            };
            await _clientUserRepo.InsertAsync(ClientUser);
            await _clientUserRepo.SaveChangesAsync();

        }

        public async Task<GetClientUserAccountResponse> GetClientUserAccount()
        {
            var userId = _currentUserService.UserId;

            var clientUser = await _clientUserRepo.GetAll()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (clientUser == null)
            {
                throw new Exception("Client user not found");
            }

            var response = new GetClientUserAccountResponse
            {
                Id = clientUser.Id,
                Name = clientUser.User.Name,
                Email = clientUser.User.Email,
                PhoneNumber = clientUser.User.PhoneNumber,
                BirthDate = clientUser.BirthDate,
            };

            return response;
        }

        public async Task RegistrationValidation(ClientUserRegistrationRequest request)
        {
            var isEmailExist = await _userRepo.GetAll().AnyAsync(u => u.Email == request.Email);
            if (isEmailExist)
            {
                throw new Exception("Email already exists");
            }
            var isPhonrNumberExist = await _userRepo.GetAll().AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (isPhonrNumberExist)
            {
                throw new Exception("Phone number already exists");
            }
        }
    }
}