using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(short Page, short PageSize) : IQuery<IEnumerable<CategoryDTO>>
{ }
