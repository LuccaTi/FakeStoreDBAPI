using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.ProcessedFileLog
{
    public class UpdateProcessedFileLogDto
    {
        [Required]
        public string? FileName { get; set; }
    }
}
