using Persistence.Entities;
using Persistence;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Common.Dtos.Product;

namespace Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbContext _context;

        public ProductService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> CreateProduct(AddProductDto addProductDto)
        {
            if (addProductDto == null ||
                string.IsNullOrEmpty(addProductDto.Name) ||
                string.IsNullOrEmpty(addProductDto.Description) ||
                string.IsNullOrEmpty(addProductDto.SKU))
                return null;

            var newProduct = new Product
            {
                Name = addProductDto.Name,
                Description = addProductDto.Description,
                SKU = addProductDto.SKU
            };

            _context.Products.Add(newProduct);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ? MapToProductDto(newProduct) : null;
        }


        public async Task<bool> DeleteProduct(int id)
        {
            var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productToDelete == null)
                return false;

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(MapToProductDto);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return product != null ? MapToProductDto(product) : null;
        }

        public async Task<ProductDto?> UpdateProduct(int id, UpdateProductDto product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Description))
                return null;

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.SKU = product.SKU;

            await _context.SaveChangesAsync();

            return MapToProductDto(existingProduct);
        }

        private ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU
            };
        }
    }
}
