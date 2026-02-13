using Application.Services.FirebaseService.DTOs;

namespace Application.Services.FirebaseService
{
    public interface IFirebaseService
    {
        Task SendAsync(List<SendFirebaseRequest> requests);
    }
}