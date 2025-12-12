using FakeStoreDBAPI.Host.DTO.Product;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllAsync();
        public Task<ProductDto?> GetByIdAsync(long id);
        public Task<ProductDto?> GetByTitleDescription(TitleDescriptionDto titleDescriptionDto);
        public Task<ProductDto> PostAsync(CreateProductDto productDto);
        public Task<ProductDto> PostWithProcessedFileLogAsync(CreateProductDto productDto, string fileName);
        public Task PatchAsync(long id, UpdateProductDto productDto);
        public Task DeleteAsync(long id);
        public Task ProductExistsAsync(long id);
    }
}
