namespace HealthProject.Models
{
    public class BookingModel
    {
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public List<string> AvailableHours { get; set; } = new List<string>();
    }
}
