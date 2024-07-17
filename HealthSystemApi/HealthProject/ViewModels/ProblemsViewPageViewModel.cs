using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.ProblemService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ProblemsViewPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProblemDisplayModel> problems;

        private IProblemService problemService;

        public ICommand AddProblemCommand { get; }
        public ICommand DeleteProblemCommand { get; }
        public ICommand NavigateToProblemDetailsCommand { get; }

        public ProblemsViewPageViewModel(IProblemService problemService)
        {
            this.problemService = problemService;

            AddProblemCommand = new AsyncRelayCommand(RedirectToAddProblemAsync);
            DeleteProblemCommand = new AsyncRelayCommand<object>(DeleteAsync);
            NavigateToProblemDetailsCommand = new AsyncRelayCommand<object>(DetailsAsync);

            LoadProblems();
        }

        private async void LoadProblems()
        {
            var problems = await problemService.GetUserProblems();

            Problems = new ObservableCollection<ProblemDisplayModel>(problems);
        }

        private async Task RedirectToAddProblemAsync()
        {
            await Shell.Current.GoToAsync("ProblemAddPage");
        }

        private async Task DeleteAsync(object parameter)
        {
            if (parameter is int id)
            {
                await problemService.DeleteAsync(id);
            }
        }

        private async Task DetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var problem = await problemService.DetailsAsync(id);

                var problemJson = JsonConvert.SerializeObject(problem);
                var encodedProblemJson = Uri.EscapeDataString(problemJson);

                await Shell.Current.GoToAsync($"ProblemDetailsPage?problemJson={encodedProblemJson}");
            }
        }
    }
}
