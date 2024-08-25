using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DiagnosisService;
using System.Collections.ObjectModel;

namespace HealthProject.ViewModels
{
    public partial class DiagnosisViewModel : ObservableObject
    {
        private readonly IDiagnosisService diagnosisService;

        public DiagnosisViewModel(IDiagnosisService diagnosisService)
        {
            this.diagnosisService = diagnosisService;
            Symptoms = new ObservableCollection<string>();
            RecommendedDoctors = new ObservableCollection<DoctorModel>();
            SymptomsInput = new SymptomsInput();
        }

        [ObservableProperty]
        private ObservableCollection<string> symptoms;

        [ObservableProperty]
        private SymptomsInput symptomsInput;

        [ObservableProperty]
        private ObservableCollection<DoctorModel> recommendedDoctors;

        [RelayCommand]
        private void AddSymptom(string symptom)
        {
            if (!string.IsNullOrWhiteSpace(symptom))
            {
                Symptoms.Add(symptom);
                SymptomsInput.SymptomInput = "";
                SymptomsInput.PredictionResult = "";
            }
        }

        [RelayCommand]
        private async Task SubmitSymptomsAsync()
        {
            if (Symptoms.Any())
            {
                var prediction = await diagnosisService.GetPrediction(Symptoms.ToList());

                SymptomsInput.PredictionResult = prediction.Prediction ?? "Error!";
                RecommendedDoctors = new ObservableCollection<DoctorModel>(prediction.RecommendedDoctors);
            }
            else
            {
                SymptomsInput.PredictionResult = "Please add at least one symptom.";
            }
        }
    }

    public class SymptomsInput
    {
        public string? SymptomInput { get; set; } = "";
        public string? PredictionResult { get; set; } = "";
    }
}