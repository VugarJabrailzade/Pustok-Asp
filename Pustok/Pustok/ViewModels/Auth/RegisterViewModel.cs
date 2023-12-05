using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Password doesn't match. Please write again")]
        public string ConfirmPassword { get; set; }

    }
}
