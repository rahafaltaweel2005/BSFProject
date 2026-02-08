namespace Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? ServiceProviderId { get; }
        int? ClientUserId { get; }
        string? Name { get; }
        string? Email { get; }
        string? MobilePhone { get; }
        string? Role { get; }
    }

}