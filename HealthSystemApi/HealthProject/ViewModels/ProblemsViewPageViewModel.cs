using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ProblemService;
using HealthProject.Views;
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
        private IAuthenticationService authenticationService;

        public ICommand AddProblemCommand { get; }
        public ICommand DeleteProblemCommand { get; }
        public ICommand NavigateToProblemDetailsCommand { get; }

        public ProblemsViewPageViewModel(IProblemService problemService,
                                         IAuthenticationService authenticationService)
        {
            this.problemService = problemService;
            this.authenticationService = authenticationService;

            AddProblemCommand = new AsyncRelayCommand(RedirectToAddProblemAsync);
            DeleteProblemCommand = new AsyncRelayCommand<object>(DeleteAsync);
            NavigateToProblemDetailsCommand = new AsyncRelayCommand<object>(DetailsAsync);

            LoadProblems();
        }

        private async void LoadProblems()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            var problems = await problemService.GetUserProblems(auth.UserId);

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
