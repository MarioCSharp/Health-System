namespace HealthSystemApi.Models.Service
{
    public class ServiceEditModel
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string? ServiceDesription { get; set; }
        public string? ServiceLocation { get; set; }
        public string? Token { get; set; }
    }
}
