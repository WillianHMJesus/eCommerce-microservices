using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task UpdateProductAsync(Product product);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page = 1, short pageSize = 10);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId);
}
