using HealthSystem.Documents.Data;
using HealthSystem.Documents.Data.Models;
using HealthSystem.Documents.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Services.DocumentService
{
    /// <summary>
    /// Provides methods for managing documents, including adding, editing, removing, and retrieving document details.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="DocumentsDbContext"/> used to interact with the database.</param>
        public DocumentService(DocumentsDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new document to the database.
        /// </summary>
        /// <param name="model">The model containing the document details.</param>
        /// <param name="userId">The user ID of the document owner.</param>
        /// <param name="File">The file to be uploaded.</param>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation, with a value indicating whether the addition was successful.</returns>
        public async Task<bool> AddAsync(DocumentAddModel model, string? userId, IFormFile File)
        {
            var document = new Document()
            {
                Notes = model.Notes,
                Title = model.Title,
                UserId = model.UserId,
                Type = model.Type,
                FileExtension = model.FileExtension
            };

            if (model.HealthIssueId != 0)
            {
                document.HealthIssueId = model.HealthIssueId;
            }

            using (var stream = new MemoryStream())
            {
                await File.CopyToAsync(stream);
                document.File = stream.ToArray();
            }

            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();

            return await context.Documents.ContainsAsync(document);
        }

        /// <summary>
        /// Retrieves all documents associated with a specific user.
        /// </summary>
        /// <param name="userId">The user ID whose documents are to be retrieved.</param>
        /// <returns>A <see cref="Task{List{DocumentDisplayModel}}"/> representing the asynchronous operation, with a list of documents.</returns>
        public async Task<List<DocumentDisplayModel>> AllByUser(string? userId)
        {
            return await context.Documents
                .Where(x => x.UserId == userId)
                .Select(x => new DocumentDisplayModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    FileName = x.File
                }).ToListAsync();
        }

        /// <summary>
        /// Retrieves the details of a specific document.
        /// </summary>
        /// <param name="id">The ID of the document to retrieve.</param>
        /// <returns>A <see cref="Task{DocumentDetailsModel}"/> representing the asynchronous operation, with the document details.</returns>
        public async Task<DocumentDetailsModel> DetailsAsync(int id)
        {
            var document = await context.Documents.FindAsync(id);

            if (document == null) return new DocumentDetailsModel();

            return new DocumentDetailsModel()
            {
                Id = document.Id,
                HealthIssueId = document.HealthIssueId,
                Title = document.Title,
                Type = document.Type,
                FileName = document.File,
                Notes = document.Notes
            };
        }

        /// <summary>
        /// Updates the details of an existing document.
        /// </summary>
        /// <param name="model">The model containing updated document details.</param>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation, with a value indicating whether the update was successful.</returns>
        public async Task<bool> EditAsync(DocumentEditModel model)
        {
            var document = await context.Documents.FindAsync(model.Id);

            if (document == null) return false;

            document.Title = model.Title;
            document.Notes = model.Notes;
            document.Type = model.Type;
            document.HealthIssueId = model.HealthIssueId;

            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Removes a document from the database.
        /// </summary>
        /// <param name="id">The ID of the document to remove.</param>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation, with a value indicating whether the removal was successful.</returns>
        public async Task<bool> RemoveAsync(int id)
        {
            var document = new Document() { Id = id };

            context.Documents.Remove(document);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
