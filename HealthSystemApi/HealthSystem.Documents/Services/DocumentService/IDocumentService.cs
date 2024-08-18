using HealthSystem.Documents.Models;

namespace HealthSystem.Documents.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<bool> AddAsync(DocumentAddModel model, string? userId, IFormFile File);
        Task<bool> EditAsync(DocumentEditModel model);
        Task<bool> RemoveAsync(int id);
        Task<DocumentDetailsModel> DetailsAsync(int id);
        Task<List<DocumentDisplayModel>> AllByUser(string? userId);
    }
}
