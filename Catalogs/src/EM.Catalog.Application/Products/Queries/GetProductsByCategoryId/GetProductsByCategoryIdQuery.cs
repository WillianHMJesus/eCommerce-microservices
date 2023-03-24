﻿using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed record GetProductsByCategoryIdQuery(Guid CategoryId) : IQuery<IEnumerable<ProductDTO>>
{ }