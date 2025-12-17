using FakeStoreDBAPI.Host.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.Models
{
    public class Product : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }
        [Required]
        [Precision(18,2)]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Category { get; set; }
        [MaxLength(255)]
        public string? Image { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
