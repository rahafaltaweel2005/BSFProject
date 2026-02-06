namespace Application.Servicess.LookupService
{
    public interface ILookupService
    {
        Task <List<GetLookupResponse>> GetAllServiceCategories();
    }
}