using Application.Services.ServiceProviderService.DTOs;
using Application.Servicess.ServiceProviderService.DTOs;

namespace Application.Servicess.ServiceProviderServices
{
    public interface IServiceProviderService
    {
        Task ServiceProviderRegistration(ServiceProviderRegistrationRequest request);
        Task<GetServiceProviderAccountResponse> GetServiceProviderAccount();
        Task UpdateServiceProviderAccount(ServiceProviderRegistrationRequest request);
    }
}