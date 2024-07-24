using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthSystemApi.Models.Document
{
    public class DocumentAddModel
    {
        [FromForm(Name = "Type")]
        public string? Type { get; set; }

        [FromForm(Name = "Title")]
        public string? Title { get; set; }

        [FromForm(Name = "Notes")]
        public string? Notes { get; set; }

        [FromForm(Name = "HealthIssueId")]
        public int HealthIssueId { get; set; }

        [FromForm(Name = "FileName")]
        public string? FileName { get; set; }

        [FromForm(Name = "FileExtension")]
        public string? FileExtension { get; set; }

        [FromForm(Name = "UserId")]
        public string? UserId { get; set; }
    }
}
