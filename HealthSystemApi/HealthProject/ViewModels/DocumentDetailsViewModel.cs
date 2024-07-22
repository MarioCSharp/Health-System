using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.DocumentService;

namespace HealthProject.ViewModels
{
    public partial class DocumentDetailsViewModel : ObservableObject
    {
        private IDocumentService documentService;

        [ObservableProperty]
        private DocumentDetailsModel document;

        public DocumentDetailsViewModel(DocumentDetailsModel model,
                                        IDocumentService documentService)
        {
            this.documentService = documentService;
            Document = model;
        }
    }
}
