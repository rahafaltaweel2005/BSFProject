using Application.Repositories;
using Application.Services.ServiceProviderService.DTOs;
using Application.Servicess.CurrentUserService;
using Application.Servicess.ServiceProviderService.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicess.ServiceProviderServices
{
    public class ServiceProviderService : IServiceProviderService
    {
        private IGenericRepository<ServiceProvider> _ServiceProviderRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly ICurrentUserService _currentUserService;
        public ServiceProviderService(IGenericRepository<ServiceProvider> ServiceProviderRepo, IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo,ICurrentUserService currentUserService)
        {
            _ServiceProviderRepo = ServiceProviderRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _currentUserService = currentUserService;
        }

        public async Task ServiceProviderRegistration(ServiceProviderRegistrationRequest request)
        {
            await RegistrationValidation(request);
            var serviceProviderRole = await _roleRepo.GetAll().FirstOrDefaultAsync(x => x.Code ==SystemRole.ServiceProvider);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RoleId = serviceProviderRole.Id
            };
            var PasswordHasher = new PasswordHasher<User>();
            user.Password = PasswordHasher.HashPassword(user, request.Password);
            await _userRepo.InsertAsync(user);
            await _userRepo.SaveChangesAsync();
            var serviceProvider = new ServiceProvider
            {
                UserId = user.Id,
                ServiceCategoryId = request.ServiceCategoryId
            };
               await _ServiceProviderRepo.InsertAsync(serviceProvider);
            await _userRepo.SaveChangesAsync();
        }
        public async Task RegistrationValidation(ServiceProviderRegistrationRequest request , int? id=null)
        {
            if (id == null)
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
            else
            {
                var isEmailExist = await _userRepo.GetAll().AnyAsync(u => u.Email == request.Email&& u.Id !=id);
            if (isEmailExist)
            {
                throw new Exception("Email already exists");
            }
            var isPhonrNumberExist = await _userRepo.GetAll().AnyAsync(u => u.PhoneNumber == request.PhoneNumber && u.Id !=id);
            if (isPhonrNumberExist)
            {
                throw new Exception("Phone number already exists");
            }
            }
            
        }

        public async Task<GetServiceProviderAccountResponse> GetServiceProviderAccount()
        {
            var userid=_currentUserService.UserId;
            var serviceProvider = await _ServiceProviderRepo.GetAll().Include(sp=>sp.User).FirstOrDefaultAsync(sp=>sp.UserId==userid);
            if (serviceProvider == null)
            {
                throw new Exception("Service provider not found");
            }
            var response= new GetServiceProviderAccountResponse
            {
                Id = serviceProvider.Id,
                Name = serviceProvider.User.Name,
                Email = serviceProvider.User.Email,
                PhoneNumber = serviceProvider.User.PhoneNumber,
                ServiceCategoryId = serviceProvider.ServiceCategoryId
            };
            return response;
        }

        public async Task UpdateServiceProviderAccount(ServiceProviderRegistrationRequest request)
        {
            var userId=_currentUserService.UserId;
            await RegistrationValidation(request, userId);
            var serviceProvider =await _ServiceProviderRepo.GetAll().Include(sp=>sp.User).FirstOrDefaultAsync(sp=>sp.UserId==userId);
            if (serviceProvider == null)           
             {
                throw new Exception("Service provider not found");
        }
            serviceProvider.User.Name = request.Name;
            serviceProvider.User.Email = request.Email;
            serviceProvider.User.PhoneNumber = request.PhoneNumber;
            serviceProvider.ServiceCategoryId = request.ServiceCategoryId;
            _ServiceProviderRepo.Update(serviceProvider);
            await _ServiceProviderRepo.SaveChangesAsync();
        }
    }
}