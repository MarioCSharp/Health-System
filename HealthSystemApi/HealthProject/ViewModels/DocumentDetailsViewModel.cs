using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.DocumentService;

namespace HealthProject.ViewModels
{
    public partial class DocumentDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private DocumentDetailsModel document;

        private IDocumentService documentService;

        public DocumentDetailsViewModel(DocumentDetailsModel model,
                                        IDocumentService documentService)
        {
            this.documentService = documentService;
            Document = model;
        }
    }
}
