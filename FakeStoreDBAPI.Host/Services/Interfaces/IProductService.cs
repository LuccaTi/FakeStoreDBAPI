using FakeStoreDBAPI.Host.DTO.Product;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
        public Task<ProductDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        public Task<ProductDto?> GetByTitleDescriptionAsync(TitleDescriptionDto titleDescriptionDto, CancellationToken cancellationToken);
        public Task<ProductDto> PostAsync(CreateProductDto productDto, CancellationToken cancellationToken);
        public Task<ProductDto> PostWithProcessedFileLogAsync(CreateProductDto productDto, string fileName, CancellationToken cancellationToken);
        public Task PatchAsync(long id, UpdateProductDto productDto, CancellationToken cancellationToken);
        public Task DeleteAsync(long id, CancellationToken cancellationToken);
        public Task ProductExistsAsync(long id, CancellationToken cancellationToken);
    }
}
