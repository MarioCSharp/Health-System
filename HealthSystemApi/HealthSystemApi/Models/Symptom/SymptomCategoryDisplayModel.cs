namespace HealthSystemApi.Models.Symptom
{
    public class SymptomCategoryDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<SymptomSubCategoryDisplayModel> SubCategories { get; set; } = new List<SymptomSubCategoryDisplayModel>();
    }
}
