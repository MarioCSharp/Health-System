using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DocumentService;
using HealthProject.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HealthProject.ViewModels
{
    public partial class DocumentAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private DocumentAddModel document;
        public IAsyncRelayCommand PickFileCommand { get; }
        public IAsyncRelayCommand AddDocumentCommand { get; }

        private IDocumentService documentService;
        private IAuthenticationService authenticationService;

        public DocumentAddViewModel(IDocumentService documentService,
                                    IAuthenticationService authenticationService)
        {
            this.documentService = documentService;
            this.authenticationService = authenticationService;
            Document = new DocumentAddModel();
            PickFileCommand = new AsyncRelayCommand(PickFileAsync);
            AddDocumentCommand = new AsyncRelayCommand(UploadFileAsync);
        }

        private async Task UploadFileAsync()
        {
            if (string.IsNullOrEmpty(Document.FilePath))
            {
                return;
            }

            try
            {
                var auth = await authenticationService.IsAuthenticated();

                if (!auth.IsAuthenticated)
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }

                var fileBytes = await File.ReadAllBytesAsync(Document.FilePath);
                var fileName = Path.GetFileName(Document.FilePath);
                var fileExtension = Path.GetExtension(Document.FilePath);

                using (var stream = new MemoryStream(fileBytes))
                {
                    IFormFile formFile = new FormFile(stream, 0, fileBytes.Length, fileName, fileName);

                    var uploadDocument = new DocumentAddModel
                    {
                        Type = Document.Type,
                        Title = Document.Title,
                        Notes = Document.Notes,
                        HealthIssueId = Document.HealthIssueId,
                        FileName = fileName,
                        FileContent = fileBytes,
                        FileExtension = fileExtension,
                        UserId = auth.UserId
                    };

                    var result = await documentService.AddAsync(uploadDocument, formFile);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
            }
        }

        private async Task PickFileAsync()
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "com.adobe.pdf", "com.microsoft.word.doc", "com.microsoft.word.docx", "public.image" } },
                    { DevicePlatform.Android, new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "image/*" } },
                    { DevicePlatform.WinUI, new[] { ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg", ".bmp" } },
                    { DevicePlatform.Tizen, new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "image/*" } },
                });

                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Моля изберете файл на документа",
                    FileTypes = customFileType
                });

                if (result != null)
                {
                    Document.FilePath = result.FullPath;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
