using AutoFixture;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Products.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EM.Catalog.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(ProductCollection))]
public class ProductCollection : ICollectionFixture<ProductFixture>
{ }

public class ProductFixture : IClassFixture<WebApplicationFactory<Program>>
{
    private ProductRequest? _productRequest;

    public Guid CategoryId { get; set; }
    public Guid? ProductId { get; set; }

    public ProductRequest GenerateValidProductRequestWithTheSameValue(Guid categoryId)
    {
        if (_productRequest is null)
        {
            _productRequest = new Fixture()
                .Build<ProductRequest>()
                .With(x => x.CategoryId, categoryId)
                .Create();
        }

        return _productRequest;
    }

    public CategoryRequest GenerateValidCategoryRequest()
    {
        return new Fixture().Create<CategoryRequest>();
    }
}
