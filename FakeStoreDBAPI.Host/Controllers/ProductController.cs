using FakeStoreDBAPI.Host.DTO.Address;
using FakeStoreDBAPI.Host.DTO.Product;
using FakeStoreDBAPI.Host.Services;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(product);
        }

        [HttpHead("{id}/product-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _productService.ProductExistsAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("title-description")]
        public async Task<IActionResult> GetByTitleDescriptionAsync([FromBody] TitleDescriptionDto titleDescriptionDto, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByTitleDescriptionAsync(titleDescriptionDto, cancellationToken);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateProductDto productDto, CancellationToken cancellationToken)
        {
            var createdProduct = await _productService.PostAsync(productDto, cancellationToken);
            return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPost("create-with-log/{fileName}")]
        public async Task<IActionResult> PostWithProcessedFileLogAsync([FromBody] CreateProductDto productDto, [FromRoute] string fileName, CancellationToken cancellationToken)
        {
            var createdProduct = await _productService.PostWithProcessedFileLogAsync(productDto, fileName, cancellationToken);
            return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateProductDto product, CancellationToken cancellationToken)
        {
            await _productService.PatchAsync(id, product, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
