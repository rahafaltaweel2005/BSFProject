using System.Linq;
using System.Security.Cryptography;
using Application.Generic_DTOs;
using Application.Repositories;
using Application.Services.Service.DTOs;
using Application.Services.CurrentUserService;
using Application.Services.LookupService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Application.Services.FileService;

namespace Application.Services.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IGenericRepository<Domain.Entities.Service> _serviceRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<ServiceProvider> _serviceProvider;
        private readonly IFileService _fileService;
        public ServicesService(IGenericRepository<Domain.Entities.Service> serviceRepository, ICurrentUserService currentUserService, IGenericRepository<ServiceProvider> _serviceProvider,IFileService fileService)
        {
            _serviceRepository = serviceRepository;
            _currentUserService = currentUserService;
            _serviceProvider = _serviceProvider;
            _fileService = fileService;
        }
        public async Task CreateService(SaveServiceRequest request)
        {
            var service = new Domain.Entities.Service
            {
                Name = request.Name,
                ServiceProviderId = _currentUserService.ServiceProviderId.Value,
                Description = request.Description,
                Price = request.Price,
                Duration = request.Duration,
                Image =await _fileService.SaveFileAsync(request.Image,"Services")
            };
            await _serviceRepository.InsertAsync(service);
            await _serviceRepository.SaveChangesAsync();
        }

        public async Task DeleteService(int id)
        {
            var service = await _serviceRepository.GetBYIdAsync(id);
            if (service == null)
            {
                throw new Exception("Service not exsits");
            }
            _serviceRepository.Delete(service);
            await _serviceRepository.SaveChangesAsync();
        }

        public async Task<PaginationResponse<GetServicesResponse>> GetAllServices(GetServicesRequest request)
        {
            var querey = _serviceRepository.GetAll().Include(x => x.ServiceProvider).OrderByDescending(x => x.Id)
            .Where(x => x.ServiceProvider.ServiceCategoryId == request.CategoryId)
            .Skip(request.PageSize * request.PageIndex).Take(request.PageSize);
            var count = await querey.CountAsync();
            var result = await querey.Select(x => new GetServicesResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Duration = x.Duration

            }).ToListAsync();
            return new PaginationResponse<GetServicesResponse>
            {
                Item = result,
                Count = count
            };
        }
        public async Task<PaginationResponse<GetServicesResponse>> GetServicesbyCategory(GetServicesRequest request)
        {
            var querey = _serviceRepository.GetAll().OrderBy(x => x.Id)
            .Where(x => x.ServiceProvider.ServiceCategoryId == request.CategoryId && x.ServiceProvider.IsAvailable );
            var count = await querey.CountAsync();
            var result = await querey.Skip(request.PageSize * request.PageIndex).Take(request.PageSize).Include(x => x.ServiceProvider).Select(x => new GetServicesResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Duration = x.Duration,
                Image = x.Image
            }).ToListAsync();
            return new PaginationResponse<GetServicesResponse>
            {
                Item = result,
                Count = count
            };
        }

        public async Task<PaginationResponse<GetServicesResponse>> GetMyServices(PaginationRequest request)
        {
            var ServiceProviderId = _currentUserService.ServiceProviderId.Value;
            var querey = _serviceRepository.GetAll().OrderByDescending(x => x.Id)
            .Where(x => x.ServiceProviderId == ServiceProviderId);
            var count = await querey.CountAsync();
            var result = await querey.Skip(request.PageSize * request.PageIndex).Take(request.PageSize).Select(x => new GetServicesResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Duration = x.Duration,
                Image = x.Image          
                }).ToListAsync();
            return new PaginationResponse<GetServicesResponse>
            {
                Item = result,
                Count = count
            };

        }

        public async Task UpdateService(int id, SaveServiceRequest request)
        {
            var service = await _serviceRepository.GetBYIdAsync(id);
            service.Name = request.Name;
            service.Description = request.Description;
            service.Price = request.Price;
            service.Duration = request.Duration;
            if (request.DeleteImage)
            {
                _fileService.DeleteFile(service.Image);
                service.Image=null;
            }
            if(request.Image != null)
            {
                _fileService.DeleteFile(service.Image);
                service.Image= await _fileService.SaveFileAsync(request.Image, "Services");
            }
            _serviceRepository.Update(service);
            await _serviceRepository.SaveChangesAsync();
        }
    }
}