using Application.Managers.Chat;
using Application.Services.CurrentUserService;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatConnectionManager _chatConnectionManager;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(IChatConnectionManager chatConnectionManager,ICurrentUserService currentUserService)
        {
            _chatConnectionManager=chatConnectionManager;
            _currentUserService=currentUserService;
        }
        public override Task OnConnectedAsync()
        {
            if (_currentUserService.UserId.HasValue)
            {
                _chatConnectionManager.Add(_currentUserService.UserId.Value.ToString(),Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_currentUserService.UserId.HasValue)
            {
                _chatConnectionManager.Remove(_currentUserService.UserId.Value.ToString());
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}