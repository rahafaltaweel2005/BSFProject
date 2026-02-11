using Application.Services.ClientUserService.DTOs;

namespace Application.Services.ClientUserService
{
    public interface IClientUserService
    {
        Task ClientUserRegistration(ClientUserRegistrationRequest request);
        Task <GetClientUserAccountResponse> GetClientUserAccount();
        Task UpdateClientUserAccount(UpdateClientUserRequest request);

    }
}