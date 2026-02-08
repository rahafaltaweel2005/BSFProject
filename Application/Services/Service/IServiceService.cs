using Application.Generic_DTOs;
using Application.Services.Service.DTOs;
using Domain.Entities;

namespace Application.Services.Service
{
    public interface IServicesService
    {
        Task <PaginationResponse<GetServicesResponse>> GetMyServices(PaginationRequest request);
        Task CreateService(SaveServiceRequest request);
        Task UpdateService(int id ,SaveServiceRequest request);
        Task DeleteService(int id);
        Task<PaginationResponse<GetServicesResponse>> GetAllServices(PaginationRequest request);
        Task<PaginationResponse<GetServicesResponse>> GetServicesbyCategory(PaginationRequest request);


    }
}