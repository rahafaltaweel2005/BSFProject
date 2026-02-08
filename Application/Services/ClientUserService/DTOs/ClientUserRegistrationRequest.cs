namespace Application.Services.ClientUserService.DTOs
{
    public class ClientUserRegistrationRequest
    {
        public DateTime BirthDate { get; set; }
         public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}