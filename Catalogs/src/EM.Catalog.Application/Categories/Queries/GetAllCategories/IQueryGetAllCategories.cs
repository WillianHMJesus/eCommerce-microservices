using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public interface IQueryGetAllCategories
{
    Task<IEnumerable<CategoryDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken);
}
