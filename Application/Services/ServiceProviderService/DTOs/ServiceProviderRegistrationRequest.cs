namespace Application.Services.ServiceProviderService.DTOs
{
    public class ServiceProviderRegistrationRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int ServiceCategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }
}