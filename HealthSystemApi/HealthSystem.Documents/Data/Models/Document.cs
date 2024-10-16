﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Documents.Data.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        [Required]
        public byte[] File { get; set; }

        [Required]
        public string FileExtension { get; set; } = null!;

        public int HealthIssueId { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
