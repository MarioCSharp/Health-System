using HealthSystem.Problems.Services.ProblemService;

namespace HealthSystem.Problems.Extensions
{
    public static class ApplicationBuilder
    {
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            await SeedSymptoms(services.GetRequiredService<IProblemService>());
        }

        private async static Task SeedSymptoms(IProblemService problemService)
        {
            await problemService.AddSymptomsAsync();
        }
    }
}
