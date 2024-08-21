using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.ProblemService;
using System.Collections.ObjectModel;

namespace HealthProject.ViewModels
{
    public partial class ProblemDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProblemDetailsModel problem;

        [ObservableProperty]
        private ObservableCollection<SymptomModel> symptoms;

        private IProblemService problemService;

        public ProblemDetailsViewModel(ProblemDetailsModel problem,
                                       IProblemService problemService)
        {
            this.problem = problem;
            this.problemService = problemService;
            Symptoms = new ObservableCollection<SymptomModel>(problem.Symptoms);
        }
    }
}
