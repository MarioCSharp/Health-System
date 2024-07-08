namespace HealthSystemApi.Models.Service
{
    public class ServiceDetailsModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public int DoctorId { get; set; }
    }
}
