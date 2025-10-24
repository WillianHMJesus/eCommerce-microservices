using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using EM.Catalog.Application.Categories;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class ProductReadRepository : IProductReadRepository
{
    private readonly ElasticsearchClient _client;

    public ProductReadRepository(ElasticsearchClient client)
    {
        _client = client;

        var indexConfig = new ElasticSearchIndexSettings(_client);
        indexConfig.CreateProductIndexAsync().Wait();
        indexConfig.CreateCategoryIndexAsync().Wait();
    }

    public async Task AddAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _client.IndexAsync(product, i => i
            .Index("products")
            .Id(product.Id)
            .Refresh(Refresh.True), cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _client.DeleteAsync<ProductDTO>(id, d => d.Index("products"), cancellationToken);
    }

    public async Task UpdateAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _client.UpdateAsync<ProductDTO, ProductDTO>(
            "products",
            product.Id,
            x => x.Doc(product).DocAsUpsert(true),
            cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<ProductDTO>(s => s
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.MatchAll(new MatchAllQuery())),
            cancellationToken);

        return response.Documents;
    }

    public async Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<ProductDTO>(s => s
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Term(t => t.Field("categoryId").Value(categoryId.ToString()))),
            cancellationToken);

        return response.Documents;
    }

    public async Task<ProductDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync<ProductDTO>(id, cancellationToken);

        return response.Source;
    }

    public async Task<IEnumerable<ProductDTO>> SearchAsync(string text, short page, short pageSize, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<ProductDTO>(s => s
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.MultiMatch(mm => mm.
                Fields(new[] { "name", "description", "category.name", "category.description" })
                .Query(text)
                .Fuzziness("AUTO"))
            ), cancellationToken);

        return response.Documents;
    }


    public async Task AddCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _client.IndexAsync(category, i => i
            .Index("categories")
            .Id(category.Id)
            .Refresh(Refresh.True), cancellationToken);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await _client.DeleteAsync<CategoryDTO>(categoryId, d => d.Index("categories"), cancellationToken);
    }

    public async Task UpdateCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _client.UpdateAsync<CategoryDTO, CategoryDTO>(
            "categories",
            category.Id,
            u => u.Doc(category).DocAsUpsert(true),
            cancellationToken);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<CategoryDTO>(s => s
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.MatchAll(new MatchAllQuery())),
            cancellationToken);

        return response.Documents;
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync<CategoryDTO>(categoryId, g => g.Index("categories"), cancellationToken);

        return response.Source;
    }
}
