﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Problems.Data.Models
{
    public class Symptom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public SymptomSubCategory? Category { get; set; }

        public ICollection<Problem> Problems { get; } = new List<Problem>();
    }
}
