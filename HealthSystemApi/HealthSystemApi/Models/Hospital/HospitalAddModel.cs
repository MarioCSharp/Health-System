using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Hospital
{
    public class HospitalAddModel
    {
        [Required]
        public string? HospitalName { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? ContactNumber { get; set; }

        [Required]
        public string? OwnerId { get; set; }

        public string? Token { get; set; }
    }
}
