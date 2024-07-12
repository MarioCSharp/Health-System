using HealthSystemApi.Models.Symptom;

namespace HealthSystemApi.Models.Problem
{
    public class ProblemDisplayModel
    {
        public string? Notes { get; set; }

        public DateTime Date { get; set; }

        public List<SymptomModel> Symptoms { get; set; } = new List<SymptomModel>();
    }
}
