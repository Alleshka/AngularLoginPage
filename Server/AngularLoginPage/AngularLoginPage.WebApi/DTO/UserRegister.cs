using System.ComponentModel.DataAnnotations;

namespace AngularLoginPage.WebApi.DTO
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Login { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[\p{L}])(?=.*\d).{2,}$", ErrorMessage = "Password must contain at least one letter, one digit, and be at least 2 characters long")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Guid ProvinceId { get; set; }
    }
}
