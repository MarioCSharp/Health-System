using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            PredictionResult = string.Empty;
            SymptomEntryText = string.Empty;
        }

        [ObservableProperty]
        private ObservableCollection<string> symptoms;

        [ObservableProperty]
        private string predictionResult;

        [ObservableProperty]
        private string symptomEntryText;

        [RelayCommand]
        private void AddSymptom()
        {
            if (!string.IsNullOrWhiteSpace(SymptomEntryText))
            {
                Symptoms.Add(SymptomEntryText);
                SymptomEntryText = string.Empty;
            }
        }

        [RelayCommand]
        private async Task SubmitSymptomsAsync()
        {
            if (Symptoms.Any())
            {
                var prediction = await diagnosisService.GetPrediction(Symptoms.ToList());
                PredictionResult = prediction;
            }
            else
            {
                PredictionResult = "Please add at least one symptom.";
            }
        }
    }
}
