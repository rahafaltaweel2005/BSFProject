namespace Application.Managers.Chat
{
    public interface IChatConnectionManager
    {
        void Add(string userId,string connectionId);
        void Remove(string userId);
        bool  TryGet(string userId, out string connectionId);
    }
}