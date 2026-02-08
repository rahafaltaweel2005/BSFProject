using Application.Services.ServiceProviderService.DTOs;
using Application.Services.ServiceProviderService.DTOs;

namespace Application.Services.ServiceProviderServices
{
    public interface IServiceProviderService
    {
        Task ServiceProviderRegistration(ServiceProviderRegistrationRequest request);
        Task<GetServiceProviderAccountResponse> GetServiceProviderAccount();
        Task UpdateServiceProviderAccount(ServiceProviderRegistrationRequest request);
    }
}