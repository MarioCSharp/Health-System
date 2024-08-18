namespace HealthSystem.HealthCare.Models
{
    public class LogDisplayModel
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public List<int> Values { get; set; } = new List<int>();

        public string? Date { get; set; }
    }
}
