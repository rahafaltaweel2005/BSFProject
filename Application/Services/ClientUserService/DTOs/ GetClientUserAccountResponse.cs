namespace Application.Services.ClientUserService.DTOs
{
    public class GetClientUserAccountResponse
    {
        public int Id { get; set; }
         public DateTime BirthDate { get; set; }
         public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}