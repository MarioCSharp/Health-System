using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Services.FileService;
using HealthProject.Services.LaboratoryService;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class LaboratoryViewModel : ObservableObject
    {
        private readonly ILaboratoryService laboratoryService;
        private readonly SaveFileService saveFileService;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isMessageVisible;

        private byte[]? _downloadedFile;
        private string? _downloadedFilePath;

        public LaboratoryViewModel(ILaboratoryService laboratoryService)
        {
            this.laboratoryService = laboratoryService;
            this.saveFileService = new SaveFileService();

            CheckResultsCommand = new AsyncRelayCommand(CheckResultsAsync);
            DownloadFileCommand = new RelayCommand(DownloadFile);
        }

        public ICommand CheckResultsCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }

        public async Task CheckResultsAsync()
        {
            IsMessageVisible = false;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                Message = "Please enter both username and password.";
                IsMessageVisible = true;
                return;
            }

            var result = await laboratoryService.CheckResult(UserName, Password);

            if (result?.File != null && result.File.Length > 0)
            {
                _downloadedFile = result.File;
                _downloadedFilePath = await saveFileService.SaveFileAsync(_downloadedFile, "lab_result.pdf");
                Message = "File is ready for download.";
            }
            else
            {
                Message = "No file found or incorrect credentials.";
                _downloadedFile = null;
            }

            IsMessageVisible = true;
        }

        public void DownloadFile()
        {
            if (_downloadedFile != null && !string.IsNullOrEmpty(_downloadedFilePath))
            {
                saveFileService.OpenFile(_downloadedFilePath);
            }
            else
            {
                Message = "No file available for download.";
                IsMessageVisible = true;
            }
        }
    }
}
