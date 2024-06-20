using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HealthSystem.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool _isBusy;

        [ObservableProperty]
        public string title;
    }
}
