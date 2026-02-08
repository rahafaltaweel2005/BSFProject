namespace Application.Services.AuthService.DTOs
{
    public class ChangeMyPasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }      
    }
}