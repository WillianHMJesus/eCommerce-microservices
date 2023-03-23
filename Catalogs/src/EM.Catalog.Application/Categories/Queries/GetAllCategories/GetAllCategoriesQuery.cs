using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(short Page, short PageSize) : IQuery<IEnumerable<CategoryDTO>>
{ }
