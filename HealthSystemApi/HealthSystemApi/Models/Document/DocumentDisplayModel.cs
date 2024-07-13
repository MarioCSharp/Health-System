namespace HealthSystemApi.Models.Document
{
    public class DocumentDisplayModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public byte[] FileName { get; set; }
    }
}
