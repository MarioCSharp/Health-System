namespace HealthProject.Services.DiagnosisService
{
    public interface IDiagnosisService
    {
        Task<string> GetPrediction(List<string> symptoms);
    }
}
