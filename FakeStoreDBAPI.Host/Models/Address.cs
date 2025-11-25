using FakeStoreDBAPI.Host.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.Models
{
    public class Address : IAuditable
    {
        [Key]
        public long Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Zipcode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
