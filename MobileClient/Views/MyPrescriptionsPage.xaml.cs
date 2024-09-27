using HealthProject.Services.FileService;
using HealthProject.ViewModels;

namespace HealthProject.Views
{
    public partial class MyPrescriptionsPage : ContentPage
    {
        private readonly MyPrescriptionsViewModel viewModel;
        private readonly SaveFileService saveFileService;

        public MyPrescriptionsPage(MyPrescriptionsViewModel viewModel)
        {
            InitializeComponent();

            Title = "Амбулаторни листове";

            BindingContext = this.viewModel = viewModel;
            this.saveFileService = new SaveFileService();

            this.viewModel.DownloadFileRequested += OnDownloadDocument;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.viewModel.LoadPrescriptions();
        }

        private async void OnDownloadDocument(byte[] file)
        {
            string extension = GetFileExtension(file);
            string fileName = $"document{extension}";

            var filePath = await saveFileService.SaveFileAsync(file, fileName);
            saveFileService.OpenFile(filePath);
        }

        private string GetFileExtension(byte[] fileBytes)
        {
            var fileSignatures = new Dictionary<string, string>
            {
                { "25504446", ".pdf" }, // PDF
                { "D0CF11E0", ".doc" }, // DOC
                { "504B0304", ".docx" }, // DOCX
                { "89504E47", ".png" }, // PNG
                { "FFD8FFE0", ".jpg" }, // JPG
                { "FFD8FFE1", ".jpg" }, // JPG
                { "FFD8FFE2", ".jpg" }, // JPG
                { "424D", ".bmp" } // BMP
            };

            string hex = BitConverter.ToString(fileBytes.Take(8).ToArray()).Replace("-", string.Empty);

            foreach (var signature in fileSignatures)
            {
                if (hex.StartsWith(signature.Key))
                {
                    return signature.Value;
                }
            }

            return ".bin";
        }

        private async void OnMedicineButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MedicationViewPage)}");
        }

        private async void OnRecordsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
        }

        private async void OnDocumentsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DocumentViewPage)}");
        }

        private async void OnPredictorButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DiagnosisPredictionPage)}");
        }
    }
}
