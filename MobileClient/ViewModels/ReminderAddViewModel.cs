using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.ReminderService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ReminderAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private ReminderAddModel reminder;

        private IReminderService reminderService;

        public ReminderAddViewModel(IReminderService reminderService)
        {
            this.reminderService = reminderService;

            Reminder = new ReminderAddModel();

            SubmitCommand = new AsyncRelayCommand(AddAsync);
        }

        public ICommand SubmitCommand { get; }

        public async Task AddAsync()
        {
            var result = await reminderService.AddAsync(Reminder);

            if (!result)
            {
                await Application.Current.MainPage.DisplayAlert("Грешка!", "Имаше грешка при добавяне на напомняне", "Ок");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(RemindersViewPage)}");
            }
        }
    }
}
