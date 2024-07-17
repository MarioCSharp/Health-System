namespace HealthSystemApi.Models.Symptom
{
    public class SymptomSubCategoryDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<SymptomDisplayModel> Symptoms { get; set; } = new List<SymptomDisplayModel>();
    }
}
