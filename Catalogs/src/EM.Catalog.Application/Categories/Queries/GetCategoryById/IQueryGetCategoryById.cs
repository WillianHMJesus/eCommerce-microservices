using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public interface IQueryGetCategoryById
{
    Task<CategoryDTO?> GetAsync(Guid id, CancellationToken cancellationToken);
}
