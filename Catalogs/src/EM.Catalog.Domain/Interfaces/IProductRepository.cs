using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IProductRepository
{
    #region WriteDatabase
    Task AddProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task UpdateProductAsync(Product product);
    #endregion

    #region ReadDatabase
    Task AddProductAsync(ProductDTO product);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page = 1, short pageSize = 10);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task UpdateProductAsync(ProductDTO product);
    #endregion
}
