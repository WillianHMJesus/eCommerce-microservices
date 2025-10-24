using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(short Page, short PageSize) : IQuery<IEnumerable<CategoryDTO>>;
