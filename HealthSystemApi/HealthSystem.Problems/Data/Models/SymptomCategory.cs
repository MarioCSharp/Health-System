using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Problems.Data.Models
{
    public class SymptomCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<SymptomSubCategory> SubCategories { get; set; } = new List<SymptomSubCategory>();
    }
}
