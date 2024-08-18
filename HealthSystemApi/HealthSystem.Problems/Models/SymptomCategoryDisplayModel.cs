namespace HealthSystem.Problems.Models
{
    public class SymptomCategoryDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<SymptomSubCategoryDisplayModel> SubCategories { get; set; } = new List<SymptomSubCategoryDisplayModel>();
    }
}
