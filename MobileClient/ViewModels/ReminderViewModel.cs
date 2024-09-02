using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.ReminderService;
using HealthProject.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ReminderViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ReminderDisplayModel> reminders;

        private IReminderService reminderService;

        public ReminderViewModel(IReminderService reminderService)
        {
            this.reminderService = reminderService;

            RedirectToAddCommand = new RelayCommand(RedirectToAdd);
            DeleteReminderCommand = new AsyncRelayCommand<object>(DeleteAsync);

            LoadReminders();
        }

        public ICommand RedirectToAddCommand { get; }
        public ICommand DeleteReminderCommand { get; }

        public async void LoadReminders()
        {
            var rems = await reminderService.AllByUser();

            Reminders = new ObservableCollection<ReminderDisplayModel>(rems);
        }

        public async void RedirectToAdd()
        {
            await Shell.Current.GoToAsync($"{nameof(RemindersAddPage)}");
        }

        public async Task DeleteAsync(object parameter)
        {
            if (parameter is int id)
            {
                var result = await reminderService.DeleteAsync(id);

                if (!result)
                {
                    await Application.Current.MainPage.DisplayAlert("Грешка!", "Имаше грешка при изтриване на напомняне", "Ок");
                }
                else
                {
                    LoadReminders();
                }
            }
        }
    }
}
