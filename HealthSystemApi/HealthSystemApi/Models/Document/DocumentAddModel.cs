using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthSystemApi.Models.Document
{
    public class DocumentAddModel
    {
        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        public string? FilePath { get; set; }

        public string? FileName { get; set; }

        public string? FileContent { get; set; }

        public string? FileExtension { get; set; }

        public string? UserId { get; set; }

        public int HealthIssueId { get; set; }

        public IFormFile ToFormFile()
        {
            if (FileContent == null)
            {
                throw new InvalidOperationException("FileContent cannot be null");
            }

            var byteArray = Encoding.UTF8.GetBytes(FileContent);
            var stream = new MemoryStream(byteArray);

            return new FormFile(stream, 0, byteArray.Length, FileName, $"{FileName}.{FileExtension}")
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType(FileExtension)
            };
        }

        private string GetContentType(string? extension)
        {
            if (extension == null)
            {
                return "application/octet-stream";
            }

            var contentTypeMappings = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".bmp", "image/bmp" }
            };

            return contentTypeMappings.TryGetValue(extension.ToLower(), out var contentType)
                ? contentType
                : "application/octet-stream";
        }
    }
}
