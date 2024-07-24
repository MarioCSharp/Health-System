using HealthProject.Models;
using Microsoft.AspNetCore.Http;

namespace HealthProject.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<bool> AddAsync(DocumentAddModel model, IFormFile file);
        Task<List<DocumentViewModel>> GetUserDocuments(string userId);
        Task RemoveAsync(int id);
        Task<DocumentDetailsModel> DetailsAsync(int id);
    }
}
