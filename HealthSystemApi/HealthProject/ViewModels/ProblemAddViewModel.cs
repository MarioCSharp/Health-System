using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.ProblemService;
using System.Collections.ObjectModel;

namespace HealthProject.ViewModels
{
    public partial class ProblemAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<SymptomCategoryDisplayModel> categories;

        [ObservableProperty]
        private ObservableCollection<SymptomSubCategoryDisplayModel> subCategories;

        [ObservableProperty]
        private ObservableCollection<SymptomDisplayModel> symptoms;

        [ObservableProperty]
        private ProblemAddModel problem;

        private IProblemService problemService;

        public ProblemAddViewModel(IProblemService problemService)
        {
            Categories = new ObservableCollection<SymptomCategoryDisplayModel>();
            SubCategories = new ObservableCollection<SymptomSubCategoryDisplayModel>();
            Symptoms = new ObservableCollection<SymptomDisplayModel>();
            Problem = new ProblemAddModel();

            this.problemService = problemService;

            LoadData();
        }

        private async void LoadData()
        {
            var categories = await problemService.GetSymptomsCategories();
            var subCategories = await problemService.GetSymptomsSubCategories();

            Categories = new ObservableCollection<SymptomCategoryDisplayModel>(categories);
            SubCategories = new ObservableCollection<SymptomSubCategoryDisplayModel>(subCategories);
        }

        public void OnCategorySelected(SymptomCategoryDisplayModel category)
        {
            SubCategories = category.SubCategories;
            Symptoms.Clear();
        }

        public void OnSubCategorySelected(SymptomSubCategoryDisplayModel subCategory)
        {
            Symptoms = subCategory.Symptoms;
        }

        public void OnSymptomSelected(SymptomDisplayModel symptom)
        {
            if (!Problem.SelectedSymptoms.Contains(symptom.Id))
            {
                Problem.SelectedSymptoms.Add(symptom.Id);
            }
        }
    }
}
