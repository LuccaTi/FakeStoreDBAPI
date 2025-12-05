using AutoMapper;
using FakeStoreDBAPI.Host.Data;
using FakeStoreDBAPI.Host.DTO.Product;
using FakeStoreDBAPI.Host.Exceptions;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeStoreDBAPI.Host.Services
{
    public class ProductService : IProductService
    {
        private const string _className = "ProductService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(FakeStoreDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            _logger.LogDebug($"{_className} - Attempting to obtain all product records");
            var products = await _context.Products.Where(o => o.IsActive).ToListAsync();
            if (products.Count != 0)
            {
                _logger.LogDebug($"{_className} - Records obtained: {products.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - List of records is empty!");
            }
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to find product with ID: {id}");
            if (id == 0)
                throw new InvalidIdException("Product with ID cannot be zero!");

            var product = await _context.Products.FindAsync(id);
            if (product == null || !product.IsActive)
                throw new NotFoundException($"Product with ID: {id} was not found!");

            _logger.LogDebug($"{_className} - Found product with ID: {id}");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> GetByTitleDescription(TitleDescriptionDto titleDescriptionDto)
        {
            _logger.LogDebug($"{_className} - Attempting to obtain product with title: '{titleDescriptionDto.Title}' and description: '{titleDescriptionDto.Description}'");
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Title == titleDescriptionDto.Title && p.Description == titleDescriptionDto.Description && p.IsActive);
            if (product == null)
                throw new NotFoundException($"Product with title: '{titleDescriptionDto.Title}' and description: '{titleDescriptionDto.Description}' was not found!");

            _logger.LogDebug($"{_className} - Found product with title: '{titleDescriptionDto.Title}' and description: '{titleDescriptionDto.Description}'");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> PostAsync(CreateProductDto productDto)
        {
            _logger.LogDebug($"{_className} - Attempting to post product");
            var productToPost = _mapper.Map<Product>(productDto);
            _context.Products.Add(productToPost);
            await _context.SaveChangesAsync();

            var postedProduct = _mapper.Map<ProductDto>(productToPost);
            _logger.LogDebug($"{_className} - Posted product with ID: {postedProduct.Id}");
            return postedProduct;
        }

        public async Task PatchAsync(long id, UpdateProductDto productDto)
        {
            _logger.LogDebug($"{_className} - Attempting to patch product with ID: {id}");
            if (id == 0)
                throw new InvalidIdException("Product ID cannot be zero!");

            if (productDto.Price == 0)
                throw new InvalidResourceException("Product price cannot be zero!");

            var productToPatch = await _context.Products.FindAsync(id);
            if (productToPatch == null || !productToPatch.IsActive)
                throw new NotFoundException($"Product with ID: {id} was not found");

            _mapper.Map(productDto, productToPatch);
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Patched product with ID: {id}");

        }
        public async Task DeleteAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to deactivate product with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Product ID cannot be zero!");

            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete == null || !productToDelete.IsActive)
                throw new NotFoundException($"Product with ID: {id} was not found");

            productToDelete.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Successfully deactivated product with ID: {id}");
        }

        public async Task ProductExistsAsync(long id)
        {
            _logger.LogDebug($"{_className} - Checking if product with ID: {id} exists");
            if (id == 0)
                throw new InvalidIdException("Product ID cannot be zero!");

            bool productExists = false;
            productExists = await _context.Products.AnyAsync(p => p.Id == id && p.IsActive);
            if (!productExists)
                throw new NotFoundException($"Product with ID: {id} does not exists or is inactive");

            _logger.LogDebug($"{_className} - Product with ID: {id} exists and is active");
        }
    }
}
