using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = null!;
    }
}
