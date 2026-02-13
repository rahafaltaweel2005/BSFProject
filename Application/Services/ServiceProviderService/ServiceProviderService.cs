using Application.Repositories;
using Application.Services.ServiceProviderService.DTOs;
using Application.Services.CurrentUserService;
using Application.Services.ServiceProviderService.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Services.FileService;
using Application.Services.NotificationService.DTOs;
using Application.Services.NotificationService;

namespace Application.Services.ServiceProviderServices
{
    public class ServiceProviderService : IServiceProviderService
    {
        private IGenericRepository<ServiceProvider> _ServiceProviderRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;
        private readonly INotificationService __notificationService;
        public ServiceProviderService(IGenericRepository<ServiceProvider> ServiceProviderRepo, IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo,ICurrentUserService currentUserService,IFileService fileService,INotificationService __notificationService)
        {
            _ServiceProviderRepo = ServiceProviderRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _currentUserService = currentUserService;
            _fileService=fileService;
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
                ServiceCategoryId = request.ServiceCategoryId,

            };   
               await _ServiceProviderRepo.InsertAsync(serviceProvider);
            await _userRepo.SaveChangesAsync();
             await __notificationService.SendNotification(new CreateNotificationRequest
            {
                UserId = _currentUserService.ServiceProviderId.Value,
                Title = "Account Registration",
                Message = $"Hi {user.Name}, Account created successfully.",
                Data = new Dictionary<string, string>
                {
                    { "{UserName}", user.Name },
                }
            });
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
                ServiceCategoryId = serviceProvider.ServiceCategoryId,
                IsAvailable= serviceProvider.IsAvailable
            };
            return response;
        }

        public async Task UpdateServiceProviderAccount(UpdateServiceProviderRequest request)
        {
            var userId=_currentUserService.UserId;
            await RegistrationValidation(request, userId.Value);
            var user=await _userRepo.GetBYIdAsync(userId.Value);
            var serviceProvider =await _ServiceProviderRepo.GetAll().Include(sp=>sp.User).FirstOrDefaultAsync(sp=>sp.UserId==userId);
            if (serviceProvider == null)           
             {
                throw new Exception("Service provider not found");
        }
            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
              _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            serviceProvider.ServiceCategoryId = request.ServiceCategoryId;
            serviceProvider.IsAvailable = request.IsAvailable;
            if (request.DeletePhoto)
            {
                _fileService.DeleteFile(user.PersonalPhoto);
            }
            if (request.PersonalPhoto != null)
            {
                _fileService.DeleteFile(user.PersonalPhoto);
                user.PersonalPhoto= await _fileService.SaveFileAsync(request.PersonalPhoto,"Users");
            }
            _ServiceProviderRepo.Update(serviceProvider);
            await _ServiceProviderRepo.SaveChangesAsync();
              await __notificationService.SendNotification(new CreateNotificationRequest
            {
                UserId = _currentUserService.UserId.Value,
                Title = "Update Account",
                Message = $"Hi {user.Name}, Your account has been updated successfully.",
                Data = new Dictionary<string, string>
                {
                    { "{UserName}", user.Name },
                }
            });
        }
    }
}