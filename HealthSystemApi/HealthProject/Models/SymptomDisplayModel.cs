using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthProject.Models
{
    public class SymptomDisplayModel : ObservableObject
    {
        private bool _isSelected;
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
