using Common.Dtos.Product;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductById(int id);
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> CreateProduct(AddProductDto product);
        Task<ProductDto?> UpdateProduct(int id, UpdateProductDto product);
        Task<bool> DeleteProduct(int id);
    }
}
