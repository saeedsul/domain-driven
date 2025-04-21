using Common.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("create-product")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(AddProductDto addProductDto)
        {
            var result = await _productService.CreateProduct(addProductDto);

            return result == null ? BadRequest() : CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result); // Return 400 Bad Request if order is null, otherwise return 201 Created.
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var productDto = await _productService.GetProductById(id);

            return productDto == null ? NotFound() : Ok(productDto); // Return 404 Not Found if order is not found, otherwise return 200 OK.
        }

        [HttpGet("get-all-products", Name = "GetAllProducts")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            return await _productService.UpdateProduct(id, updateProductDto) == null ? NotFound() : NoContent(); // Return 404 Not Found if order is not found, otherwise return 204 No Content.
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return await _productService.DeleteProduct(id) ? NoContent() : NotFound(); // Return 404 Not Found if order is not found, otherwise return 204 No Content.
        }
    }
}
