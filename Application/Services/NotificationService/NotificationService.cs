using Application.Generic_DTOs;
using Application.Repositories;
using Application.Services.CurrentUserService;
using Application.Services.FirebaseService;
using Application.Services.FirebaseService.DTOs;
using Application.Services.NotificationService.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepo;
        private readonly ICurrentUserService _currentUserService;
                private readonly IFirebaseService _firebaseService;
        private readonly IGenericRepository<FirebaseToken> _firebaseTokenRepo;


        public NotificationService(IGenericRepository<Notification> notificationRepo, ICurrentUserService currentUserService,IGenericRepository<FirebaseToken> firebaseTokenRepo,IFirebaseService firebaseService)
        {
            _notificationRepo = notificationRepo;
            _currentUserService = currentUserService;
            _firebaseTokenRepo=firebaseTokenRepo;
            _firebaseService=firebaseService;
        }

        public async Task<PaginationResponse<GetNotificationResponse>> GetUserNotifications(PaginationRequest request)
        {
            var query = _notificationRepo.GetAll().OrderByDescending(x => x.CreatedData)
            .Where(x => x.UserId == _currentUserService.UserId.Value);
            var count =await query.CountAsync();
            var result = await query.Skip(request.PageSize * request.PageIndex).Take(request.PageSize).Select(x => new GetNotificationResponse
            {
                UserId = x.UserId,
                OrderId = x.OrderId,
                Title = x.Title,
                Message = x.Message,
                CreatedData = DateTime.UtcNow,
                IsRead = x.IsRead
            }).ToListAsync();
            return new PaginationResponse<GetNotificationResponse>
            {
                Item = result,
                Count = count
            };
        }

        public async Task SendNotification(CreateNotificationRequest request)
        {
            var Notification = new Notification
            {
                UserId = request.UserId,
                OrderId = request.OrderId,
                Title = request.Title,
                Message = request.Message,
                CreatedData = DateTime.UtcNow,
                IsRead = false
            };
            await _notificationRepo.InsertAsync(Notification);
            await _notificationRepo.SaveChangesAsync();
             var tokens = await _firebaseTokenRepo.GetAll()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.Token)
                .ToListAsync();

            await _firebaseService.SendAsync(new List<SendFirebaseRequest>
             {
                 new SendFirebaseRequest
                 {
                     Tokens = tokens,
                     Title = request.Title,
                     Body =  request.Message,
                     Data = request.Data
                 }
             });
        }

        public async Task UpdateAllIsReaded()
        {

            var Notification = await _notificationRepo.GetAll().Where(x => x.UserId == _currentUserService.UserId.Value && !x.IsRead).ToListAsync();
            foreach (var notification in Notification)
            {
                notification.IsRead = true;
                _notificationRepo.Update(notification);
            }

            await _notificationRepo.SaveChangesAsync();
        }

        public async Task UpdateIsReaded(int NotificationId)
        {
            var Notification = await _notificationRepo.GetBYIdAsync(NotificationId);
            Notification.IsRead = true;
            _notificationRepo.Update(Notification);
            await _notificationRepo.SaveChangesAsync();

        }

    }
}