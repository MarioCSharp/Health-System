using System.Collections.ObjectModel;

namespace HealthProject.Models
{
    public class SymptomSubCategoryDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ObservableCollection<SymptomDisplayModel> Symptoms { get; set; } = new ObservableCollection<SymptomDisplayModel>();
    }
}
