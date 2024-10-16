﻿namespace HealthProject.Models
{
    public class HealthIssueAddModel
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime IssueStartDate { get; set; }

        public DateTime IssueEndDate { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
