﻿using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Models
{
    public class HealthIssueEditModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime IssueStartDate { get; set; }

        [Required]
        public DateTime IssueEndDate { get; set; }
    }
}
