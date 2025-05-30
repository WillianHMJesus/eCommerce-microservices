﻿using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) 
    : IQuery<CategoryDTO?>
{ }
