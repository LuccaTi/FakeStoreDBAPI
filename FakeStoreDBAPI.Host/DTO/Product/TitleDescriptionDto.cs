using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Product
{
    public class TitleDescriptionDto
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
