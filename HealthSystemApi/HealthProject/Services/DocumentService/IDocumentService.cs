using HealthProject.Models;

namespace HealthProject.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<List<DocumentViewModel>> GetUserDocuments(string userId);
        Task RemoveAsync(int id);
        Task<DocumentDetailsModel> DetailsAsync(int id);
    }
}
