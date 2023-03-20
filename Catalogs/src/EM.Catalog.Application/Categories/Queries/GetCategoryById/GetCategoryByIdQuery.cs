using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDTO?>
{ }
