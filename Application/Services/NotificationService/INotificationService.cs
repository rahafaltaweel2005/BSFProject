using Application.Generic_DTOs;
using Application.Services.NotificationService.DTOs;

namespace Application.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotification(CreateNotificationRequest request);
        Task UpdateIsReaded(int NotificationId);
        Task UpdateAllIsReaded();
        Task<PaginationResponse<GetNotificationResponse>> GetUserNotifications(PaginationRequest request);

    }
}