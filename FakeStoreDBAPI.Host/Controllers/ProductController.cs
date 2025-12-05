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
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpHead("{id}/product-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] long id)
        {
            await _productService.ProductExistsAsync(id);
            return NoContent();
        }

        [HttpPost("title-description")]
        public async Task<IActionResult> GetByTitleDescription([FromBody] TitleDescriptionDto titleDescriptionDto)
        {
            var product = await _productService.GetByTitleDescription(titleDescriptionDto);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateProductDto productDto)
        {
            var createdProduct = await _productService.PostAsync(productDto);
            return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateProductDto product)
        {
            await _productService.PatchAsync(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
