using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Identity.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = null!;
    }
}
