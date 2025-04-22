namespace Presentation.ViewModel.Auth
{
    public class ResetPasswordRequestVM
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string OtpCode { get; set; }

    }
}
