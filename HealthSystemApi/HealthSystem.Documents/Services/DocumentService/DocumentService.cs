﻿using HealthSystem.Documents.Data;
using HealthSystem.Documents.Data.Models;
using HealthSystem.Documents.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        private DocumentsDbContext context;

        public DocumentService(DocumentsDbContext context)
        {
            this.context = context;
        }

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

        public async Task<bool> RemoveAsync(int id)
        {
            var document = new Document() { Id = id };

            context.Documents.Remove(document);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
