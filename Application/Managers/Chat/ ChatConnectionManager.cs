namespace Application.Managers.Chat
{
    public class ChatConnectionManager : IChatConnectionManager
    {
        private readonly Dictionary<string,string> _connections= new Dictionary<string,string>();
        public void Add(string userId, string connectionId)
        {
            _connections[userId]=connectionId;
        }

        public void Remove(string userId)
        {
          _connections.Remove(userId);
        }

        public bool TryGet(string userId, out string connectionId)
        {
            return _connections.TryGetValue(userId , out connectionId);
        }
    }
}