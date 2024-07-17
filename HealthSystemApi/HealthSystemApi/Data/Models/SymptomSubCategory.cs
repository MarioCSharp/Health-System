using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
{
    public class SymptomSubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public SymptomCategory? Category { get; set; }

        public ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
    }
}
