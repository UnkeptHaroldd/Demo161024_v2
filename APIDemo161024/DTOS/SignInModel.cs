using System.ComponentModel.DataAnnotations;

namespace APIDemo161024.DTOS
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
