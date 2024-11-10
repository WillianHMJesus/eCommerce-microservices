using AutoFixture;
using EM.Carts.Application.UseCases.AddItem;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EM.Carts.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(CartCollection))]
public class CartCollection : ICollectionFixture<CartFixture>
{ }

public class CartFixture : IClassFixture<WebApplicationFactory<Program>>
{
    private AddItemRequest? _addItemRequest;

    public AddItemRequest GenerateValidAddItemRequestWithTheSameValue()
    {
        if (_addItemRequest == null)
        {
            _addItemRequest = new Fixture().Create<AddItemRequest>();
        }

        return _addItemRequest;
    }
}
