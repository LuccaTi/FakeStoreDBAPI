using FakeStoreDBAPI.Host.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.Models
{
    public class Product : IAuditable
    {
        [Key]
        public long Id { get; set; }
        public string? Title { get; set; }
        [Precision(18,2)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
