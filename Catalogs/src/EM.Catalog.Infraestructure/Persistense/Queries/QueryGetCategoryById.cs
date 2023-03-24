﻿using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Queries;

public class QueryGetCategoryById : IQueryGetCategoryById
{
    private readonly ReadContext _readContext;

    public QueryGetCategoryById(ReadContext readContext)
        => _readContext = readContext;

    public async Task<CategoryDTO> GetAsync(Guid id)
    {
        return await _readContext.Categories.Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}