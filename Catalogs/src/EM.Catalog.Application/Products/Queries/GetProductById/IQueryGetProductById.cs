using EM.Catalog.Application.DTOs;


namespace EM.Catalog.Application.Products.Queries.GetProductById;

public interface IQueryGetProductById
{
    Task<ProductDTO> GetAsync(Guid id);
}
