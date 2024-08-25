namespace HealthProject.Models
{
    public class PredictionModel
    {
        public string? Prediction { get; set; }

        public List<DoctorModel> RecommendedDoctors { get; set; } = new List<DoctorModel>();
    }
}
