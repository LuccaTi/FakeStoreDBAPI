using FakeStoreDBAPI.Host.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.Models
{
    public class ProcessedFileLog : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? FileName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get;set;}
    }
}
