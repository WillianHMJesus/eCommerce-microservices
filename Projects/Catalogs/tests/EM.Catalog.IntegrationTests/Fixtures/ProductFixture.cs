using AutoFixture;
using EM.Catalog.Application.Products.Commands.AddProduct;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EM.Catalog.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(ProductCollection))]
public class ProductCollection : ICollectionFixture<ProductFixture>
{ }

public class ProductFixture : IClassFixture<WebApplicationFactory<Program>>
{
    private AddProductCommand? _addProductCommand;

    public Guid? ProductId { get; set; }

    public AddProductCommand GenerateValidAddProductCommandWithTheSameValue(Guid categoryId)
    {
        if (_addProductCommand is null)
        {
            _addProductCommand = new Fixture()
                .Build<AddProductCommand>()
                .With(x => x.CategoryId, categoryId)
                .Create();
        }

        return _addProductCommand;
    }
}
