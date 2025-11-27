using FakeStoreDBAPI.Host.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStoreDBAPI.Host.Models
{
    public class Customer : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Address))]
        public long AddressId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public Address? Address { get; set; }
    }
}
