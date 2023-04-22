using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public interface IQueryGetProductsByCategoryId
{
    Task<IEnumerable<ProductDTO>> GetAsync(Guid categoryId, CancellationToken cancellationToken);
}
