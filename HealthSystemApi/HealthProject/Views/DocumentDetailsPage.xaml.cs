using HealthProject.Services.DocumentService;
using HealthProject.Services.FileService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

[QueryProperty(nameof(DocumentId), "docId")]
public partial class DocumentDetailsPage : ContentPage
{
    private int documentId;
    private IDocumentService documentService;
    private readonly SaveFileService saveFileService;

    public int DocumentId
    {
        get => documentId;
        set
        {
            documentId = value;
            LoadDocument();
        }
    }

    public DocumentDetailsPage(IDocumentService documentService)
    {
        InitializeComponent();
        this.documentService = documentService;
        this.saveFileService = new SaveFileService();
    }

    private async void LoadDocument()
    {
        var document = await documentService.DetailsAsync(documentId);

        if (document != null)
        {
            BindingContext = new DocumentDetailsViewModel(document, documentService);
        }
    }


    private async void OnDownloadDocument(object sender, EventArgs e)
    {
        var document = ((DocumentDetailsViewModel)BindingContext).Document;

        string extension = GetFileExtension(document.FileName);

        string fileName = $"document{extension}";

        var filePath = await saveFileService.SaveFileAsync(document.FileName, fileName);

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
}
