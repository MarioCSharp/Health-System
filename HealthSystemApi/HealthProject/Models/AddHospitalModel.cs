using System.ComponentModel.DataAnnotations;

namespace HealthProject.Models
{
    public class AddHospitalModel
    {
        public string? HospitalName { get; set; }

        public string? Location { get; set; }

        public string? ContactNumber { get; set; }

        public string? OwnerId { get; set; }
    }
}
