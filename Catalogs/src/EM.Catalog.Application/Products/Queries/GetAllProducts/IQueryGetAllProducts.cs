using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public interface IQueryGetAllProducts
{
    Task<IEnumerable<ProductDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken);
}
