using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
{
    private readonly IReadRepository _readRepository;

    public GetAllCategoriesHandler(IReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _readRepository.GetAllCategoriesAsync(query.Page, query.PageSize, cancellationToken);
    }
}
