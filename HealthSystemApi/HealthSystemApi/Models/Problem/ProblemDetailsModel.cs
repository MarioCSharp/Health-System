﻿using HealthSystemApi.Models.Symptom;
using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Problem
{
    public class ProblemDetailsModel
    {
        public int Id { get; set; }

        public string? Notes { get; set; }

        public DateTime Date { get; set; }

        public string? HealthIssueName { get; set; }

        public List<SymptomModel> Symptoms { get; set; } = new List<SymptomModel>();
    }
}
