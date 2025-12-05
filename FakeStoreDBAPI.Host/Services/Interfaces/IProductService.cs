using FakeStoreDBAPI.Host.DTO.Product;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllAsync();
        public Task<ProductDto?> GetByIdAsync(long id);
        public Task<ProductDto?> GetByTitleDescription(TitleDescriptionDto titleDescriptionDto);
        public Task<ProductDto> PostAsync(CreateProductDto addressDto);
        public Task PatchAsync(long id, UpdateProductDto addressDto);
        public Task DeleteAsync(long id);
        public Task ProductExistsAsync(long id);
    }
}
