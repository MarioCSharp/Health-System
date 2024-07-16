﻿using HealthSystemApi.Models.Symptom;

namespace HealthSystemApi.Models.Problem
{
    public class ProblemDisplayModel
    {
        public int Id { get; set; }
        public string? Notes { get; set; }

        public DateTime Date { get; set; }
    }
}
