using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthProject.ViewModels
{
    public partial class ReceptionChatViewModel : ObservableObject
    {
        [ObservableProperty]
        private int hospitalId;
    }
}
