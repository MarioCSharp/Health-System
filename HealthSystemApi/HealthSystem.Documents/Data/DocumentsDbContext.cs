using HealthSystem.Documents.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Data
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}
