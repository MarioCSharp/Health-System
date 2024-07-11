using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Data.Models
{
    public class SymptomCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
