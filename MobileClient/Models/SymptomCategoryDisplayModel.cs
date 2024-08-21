using System.Collections.ObjectModel;

namespace HealthProject.Models
{
    public class SymptomCategoryDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ObservableCollection<SymptomSubCategoryDisplayModel> SubCategories { get; set; } = new ObservableCollection<SymptomSubCategoryDisplayModel>();
    }
}
