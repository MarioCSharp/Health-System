using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Symptom
{
    public class SymptomModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? CategoryName { get; set; }
    }
}
