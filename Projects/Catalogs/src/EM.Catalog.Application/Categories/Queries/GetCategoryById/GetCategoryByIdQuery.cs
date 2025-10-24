using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDTO?>;
