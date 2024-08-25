using HealthProject.Models;

namespace HealthProject.Services.DiagnosisService
{
    public interface IDiagnosisService
    {
        Task<PredictionModel> GetPrediction(List<string> symptoms);
    }
}
