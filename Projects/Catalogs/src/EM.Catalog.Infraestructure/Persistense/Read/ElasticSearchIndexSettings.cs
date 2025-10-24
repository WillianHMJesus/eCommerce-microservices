using Elastic.Clients.Elasticsearch;
using EM.Catalog.Application.Categories;
using EM.Catalog.Application.Products;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class ElasticSearchIndexSettings(ElasticsearchClient client)
{
    public async Task CreateProductIndexAsync()
    {
        var exists = await client.Indices.ExistsAsync("products");

        if (exists.Exists)
        {
            return;
        }

        var response = await client.Indices.CreateAsync("products", c => c
            .Mappings(m => m.Properties<ProductDTO>(ps => ps
                .Keyword(k => k.Id, keyword => keyword.Index(false))
                .Keyword(k => k.Name, keyword => keyword.IgnoreAbove(Product.NameMaxLenght))
                .Keyword(k => k.Description, keyword => keyword.IgnoreAbove(Product.DescriptionMaxLenght))
                .Object(o => o.Category, c => c.Properties(p => p
                    .Keyword(k => k.Id, keyword => keyword.Index(false))
                    .Keyword(k => k.Name, keyword => keyword.IgnoreAbove(Category.NameMaxLenght))
                    .Keyword(k => k.Description, keyword => keyword.IgnoreAbove(Category.DescriptionMaxLenght))
                ))
            ))
        );
    }

    public async Task CreateCategoryIndexAsync()
    {
        var exists = await client.Indices.ExistsAsync("categories");

        if (exists.Exists)
        {
            return;
        }

        var response = await client.Indices.CreateAsync("categories", c => c
            .Mappings(m => m.Properties<CategoryDTO>(ps => ps
                .Keyword(k => k.Id, keyword => keyword.Index(false))
                .Keyword(k => k.Name, keyword => keyword.IgnoreAbove(Category.NameMaxLenght))
                .Keyword(k => k.Description, keyword => keyword.IgnoreAbove(Category.DescriptionMaxLenght))
            ))
        );
    }
}
