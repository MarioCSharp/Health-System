namespace HealthProject.Models
{
    public class SubmitOrderModel
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public int CartId { get; set; }
    }
}
