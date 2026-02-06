namespace Application.Servicess.CurrentUserService
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Name { get; }
        string? Email { get; }
        string? MobilePhone { get; }
        string? Role { get; }

    }

}