using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Hospital
{
    public class HospitalDetailsModel
    {
        public int Id { get; set; }

        public string? HospitalName { get; set; }

        public string? Location { get; set; }

        public string? ContactNumber { get; set; }
    }
}
