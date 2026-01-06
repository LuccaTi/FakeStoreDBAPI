using FakeStoreDBAPI.Host.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStoreDBAPI.Host.Models
{
    public class Order : IAuditable
    {
       
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }
        [Required]
        [MaxLength(255)]
        public string? OrderGuid { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string? OrderStatus { get; set; }
        [Required]
        [MaxLength(50)]
        public string? PaymentStatus { get; set; }
        [Required]
        [MaxLength(50)]
        public string? ShippingStatus { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public Customer? Customer { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

    }
}
