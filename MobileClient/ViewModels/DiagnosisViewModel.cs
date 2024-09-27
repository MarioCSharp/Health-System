using HealthProject.Models;
using HealthProject.Services.DiagnosisService;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class DiagnosisViewModel : INotifyPropertyChanged
{
    private readonly IDiagnosisService diagnosisService;
    private string symptomsInput;
    private string diagnosisResult;
    private ObservableCollection<string> allSymptoms;  
    private ObservableCollection<SymptomModel> filteredSymptoms;  
    private ObservableCollection<string> selectedSymptoms;
    private ObservableCollection<DoctorModel> recommendedDoctors;
    private CancellationTokenSource _debounceTimer;
    public event PropertyChangedEventHandler PropertyChanged;

    public DiagnosisViewModel(IDiagnosisService diagnosisService)
    {
        this.diagnosisService = diagnosisService;
        allSymptoms = new ObservableCollection<string>();
        filteredSymptoms = new ObservableCollection<SymptomModel>();
        selectedSymptoms = new ObservableCollection<string>();
        recommendedDoctors = new ObservableCollection<DoctorModel>();

        LoadAllSymptoms(); 

        SubmitSymptomsAsyncCommand = new Command(async () => await SubmitSymptomsAsync());
    }

    public string SymptomsInput
    {
        get => symptomsInput;
        set
        {
            symptomsInput = value;
            OnPropertyChanged(nameof(SymptomsInput));

            _debounceTimer?.Cancel();
            _debounceTimer = new CancellationTokenSource();

            Task.Delay(400, _debounceTimer.Token).ContinueWith(task =>
            {
                if (!task.IsCanceled)
                {
                    FilterSymptoms();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public ObservableCollection<SymptomModel> FilteredSymptoms
    {
        get => filteredSymptoms;
        set
        {
            filteredSymptoms = value;
            OnPropertyChanged(nameof(FilteredSymptoms));
        }
    }

    public ObservableCollection<string> SelectedSymptoms
    {
        get => selectedSymptoms;
        set
        {
            selectedSymptoms = value;
            OnPropertyChanged(nameof(SelectedSymptoms));
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

    public ICommand SubmitSymptomsAsyncCommand { get; }

    private async void LoadAllSymptoms()
    {
        try
        {
            var columns = await diagnosisService.GetSymptomsFromColumns();
            foreach (var symptom in columns)
            {
                allSymptoms.Add(symptom);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching symptoms: {ex.Message}");
        }
    }

    private void FilterSymptoms()
    {
        if (!string.IsNullOrEmpty(SymptomsInput))
        {
            var query = SymptomsInput.ToLower();
            var filtered = allSymptoms
                           .Where(s => s.ToLower().Contains(query))
                           .Take(10)
                           .Select(s => new SymptomModel { Name = s, IsSelected = false }) 
                           .ToList();

            FilteredSymptoms.Clear();
            foreach (var symptom in filtered)
            {
                FilteredSymptoms.Add(symptom);
            }
        }
    }

    private async Task SubmitSymptomsAsync()
    {
        var selectedSymptoms = FilteredSymptoms.Where(s => s.IsSelected)
                                               .Select(s => s.Name)
                                               .ToList();

        if (selectedSymptoms.Any())
        {
            var prediction = await diagnosisService.GetPrediction(selectedSymptoms);

            SymptomsInput = string.Join(", ", selectedSymptoms);
            DiagnosisResult = prediction?.Prediction ?? "Error!";
            recommendedDoctors = new ObservableCollection<DoctorModel>(prediction.RecommendedDoctors);

            OnPropertyChanged(nameof(DiagnosisResult));
            OnPropertyChanged(nameof(recommendedDoctors));
        }

        FilteredSymptoms.Select(x => x.IsSelected = false);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}