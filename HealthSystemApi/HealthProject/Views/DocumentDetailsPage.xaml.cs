using HealthProject.Models;
using HealthProject.Services.DocumentService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(DocumentJson), "documentJson")]
public partial class DocumentDetailsPage : ContentPage
{
    private string documentJson;
    private IDocumentService documentService;
    public string DocumentJson
    {
        get => documentJson;
        set
        {
            documentJson = value;
            var doc = JsonConvert.DeserializeObject<DocumentDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new DocumentDetailsViewModel(doc, documentService);
        }
    }
    public DocumentDetailsPage(IDocumentService documentService)
	{
		InitializeComponent();

        this.documentService = documentService;
	}
}