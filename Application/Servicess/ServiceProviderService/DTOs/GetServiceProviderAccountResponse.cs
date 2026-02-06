namespace Application.Services.ServiceProviderService.DTOs
{
    public class GetServiceProviderAccountResponse
    {
        public int Id { get; set; }
         public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ServiceCategoryId { get; set; }
    }
}