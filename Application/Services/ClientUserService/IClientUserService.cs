using Application.Servicess.ClientUserService.DTOs;

namespace Application.Servicess.ClientUserService
{
    public interface IClientUserService
    {
        Task ClientUserRegistration(ClientUserRegistrationRequest request);
        Task <GetClientUserAccountResponse> GetClientUserAccount();
        Task UpdateClientUserAccount(ClientUserRegistrationRequest request);

    }
}