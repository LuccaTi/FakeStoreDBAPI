using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Product
{
    public class CreateProductDto
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? Image { get; set; }
    }
}
