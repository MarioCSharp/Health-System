using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using HealthProject.Models;
using HealthProject.Services.DiagnosisService;

namespace HealthProject.ViewModels
{
    public class DiagnosisViewModel : INotifyPropertyChanged
    {
        private readonly IDiagnosisService diagnosisService;
        private string symptomsInput;
        private string diagnosisResult; 
        private ObservableCollection<DoctorModel> recommendedDoctors;

        public event PropertyChangedEventHandler PropertyChanged;

        public DiagnosisViewModel(IDiagnosisService diagnosisService)
        {
            this.diagnosisService = diagnosisService;
            RecommendedDoctors = new ObservableCollection<DoctorModel>();

            SubmitSymptomsAsyncCommand = new Command(async () => await SubmitSymptomsAsync());
        }

        public string SymptomsInput
        {
            get => symptomsInput;
            set
            {
                symptomsInput = value;
                OnPropertyChanged(nameof(SymptomsInput));
            }
        }

        public string DiagnosisResult
        {
            get => diagnosisResult;
            set
            {
                diagnosisResult = value;
                OnPropertyChanged(nameof(DiagnosisResult));
            }
        }

        public ObservableCollection<DoctorModel> RecommendedDoctors
        {
            get => recommendedDoctors;
            set
            {
                recommendedDoctors = value;
                OnPropertyChanged(nameof(RecommendedDoctors));
            }
        }

        public ICommand SubmitSymptomsAsyncCommand { get; }

        private async Task SubmitSymptomsAsync()
        {
            if (!string.IsNullOrEmpty(SymptomsInput))
            {
                var symptomsList = SymptomsInput.Split(", ", StringSplitOptions.RemoveEmptyEntries)
                                                .Select(s => s.Trim())
                                                .ToList();

                var prediction = await diagnosisService.GetPrediction(symptomsList);

                SymptomsInput = string.Join(", ", symptomsList);
                DiagnosisResult = prediction?.Prediction ?? "Error!";
                RecommendedDoctors = new ObservableCollection<DoctorModel>(prediction.RecommendedDoctors.Where(x => x.Id != 0));

                OnPropertyChanged(nameof(DiagnosisResult));  
            }
            else
            {
                SymptomsInput = "Please enter symptoms before submitting.";
                DiagnosisResult = string.Empty; 
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
