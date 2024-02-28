using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    private readonly IReadRepository _readRepository;

    public GetCategoryByIdHandler(IReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        return await _readRepository.GetCategoryByIdAsync(query.Id, cancellationToken);
    }
}
