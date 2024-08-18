using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Identity.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
