using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel.Auth
{
    public class ForgotPasswordRequestVM
    {
        [EmailAddress]
        public string EmailAddress { get; set; }


    }
}
