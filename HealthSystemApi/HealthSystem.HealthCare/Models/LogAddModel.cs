using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Models
{
    public class LogAddModel
    {
        public int Id { get; set; }

        [Required]
        [FromForm(Name = "Type")]
        public string? Type { get; set; }

        [Required]
        [FromForm(Name = "Values")]
        public List<int> Values { get; set; } = new List<int>();

        [Required]
        [FromForm(Name = "Factors")]
        public List<string> Factors { get; set; } = new List<string>();

        [FromForm(Name = "Note")]
        public string? Note { get; set; }

        [Required]
        [FromForm(Name = "UserId")]
        public string? UserId { get; set; }

        public DateTime Date { get; set; }
    }
}
