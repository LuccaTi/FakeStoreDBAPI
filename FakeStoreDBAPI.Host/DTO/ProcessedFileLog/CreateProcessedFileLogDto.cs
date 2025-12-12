using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.ProcessedFileLog
{
    public class CreateProcessedFileLogDto
    {
        [Required]
        public string? FileName { get; set; }
    }
}
