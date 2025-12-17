using FakeStoreDBAPI.Host.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.Models
{
    public class Address : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? City { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Street { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        [MaxLength(15)]
        public string? Zipcode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
