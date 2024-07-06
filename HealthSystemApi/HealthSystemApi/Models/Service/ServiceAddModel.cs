using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Service
{
    public class ServiceAddModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
