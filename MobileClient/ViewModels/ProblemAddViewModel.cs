﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ProblemService;
using HealthProject.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        private IAuthenticationService authenticationService;

        public ProblemAddViewModel(IProblemService problemService,
                                   IAuthenticationService authenticationService)
        {
            Categories = new ObservableCollection<SymptomCategoryDisplayModel>();
            SubCategories = new ObservableCollection<SymptomSubCategoryDisplayModel>();
            Symptoms = new ObservableCollection<SymptomDisplayModel>();
            Problem = new ProblemAddModel();

            ProblemAddCommand = new AsyncRelayCommand(AddAsync);
            OnSymptomTappedCommand = new RelayCommand<SymptomDisplayModel>(OnSymptomSelected);

            this.problemService = problemService;
            this.authenticationService = authenticationService;

            problem.Date = DateTime.Now;

            LoadData();
        }

        public ICommand ProblemAddCommand { get; }
        public ICommand OnSymptomTappedCommand { get; }

        public async Task AddAsync()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var result = await problemService.AddAsync(Problem, Problem.SelectedSymptoms, auth.UserId ?? string.Empty);

            await Shell.Current.GoToAsync($"///{nameof(ProblemsViewPage)}");
        }

        public async void LoadData()
        {
            var categories = await problemService.GetSymptomsCategories();

            Categories = new ObservableCollection<SymptomCategoryDisplayModel>(categories);
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
            symptom.IsSelected = !symptom.IsSelected;

            if (symptom.IsSelected)
            {
                if (!Problem.SelectedSymptoms.Contains(symptom.Id))
                {
                    Problem.SelectedSymptoms.Add(symptom.Id);
                }
            }
            else
            {
                if (Problem.SelectedSymptoms.Contains(symptom.Id))
                {
                    Problem.SelectedSymptoms.Remove(symptom.Id);
                }
            }
        }
    }
}
