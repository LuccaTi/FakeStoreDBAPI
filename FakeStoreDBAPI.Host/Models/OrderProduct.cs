using FakeStoreDBAPI.Host.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStoreDBAPI.Host.Models
{
    public class OrderProduct : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }
        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Precision(18,2)]
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
