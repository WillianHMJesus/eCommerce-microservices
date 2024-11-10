using AutoFixture.Xunit2;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.RemoveItemQuantity;
using EM.Carts.IntegrationTests.Fixtures;
using EM.Carts.IntegrationTests.Helpers;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Priority;

namespace EM.Carts.IntegrationTests.API;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection(nameof(CartCollection))]
public sealed class CartsControllerTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly CartFixture _fixture;
    private readonly HttpClient _client;
    private readonly HttpResponseMessageHelper _helper;

    public CartsControllerTest(IntegrationTestWebAppFactory factory,
        CartFixture fixture)
    {
        _client = factory.CreateClient();
        _helper = new HttpResponseMessageHelper();
        _fixture = fixture;
    }

    [Theory, AutoData, Priority(0)]
    public async Task AddItemQuantityAsync_CartNotFound_ShouldReturnBadRequest(AddItemQuantityRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/AddQuantity/{request.ProductId}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Carrinho não encontrado.");
    }

    [Theory, AutoData, Priority(0)]
    public async Task RemoveItemQuantityAsync_CartNotFound_ShouldReturnBadRequest(RemoveItemQuantityRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/RemoveQuantity/{request.ProductId}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Carrinho não encontrado.");
    }

    [Theory, AutoData, Priority(0)]
    public async Task DeleteItemAsync_CartNotFound_ShouldReturnBadRequest(Guid productId)
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts/Item/{productId}");
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Carrinho não encontrado.");
    }

    [Fact, Priority(0)]
    public async Task DeleteAllItemsAsync_CartNotFound_ShouldReturnBadRequest()
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts");
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Carrinho não encontrado.");
    }

    [Fact, Priority(1)]
    public async Task AddItemAsync_InvalidItem_ShouldReturnBadRequest()
    {
        AddItemRequest request = new()
        {
            ProductId = Guid.Empty,
            Quantity = 0
        };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/Carts/Item", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
        errors.Should().Contain(x => x.Message == "A quantidade do produto não pode ser menor ou igual a zero.");
    }

    [Fact, Priority(1)]
    public async Task AddItemAsync_AddNewItem_ShouldReturnNoContent()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/Carts/Item", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(2)]
    public async Task GetCartAsync_CartFound_ShouldReturnOnlyOneItem()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(1);
    }

    [Fact, Priority(3)]
    public async Task AddItemAsync_AddQuantityExistingItem_ShouldReturnNoContent()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/Carts/Item", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(4)]
    public async Task GetCartAsync_CartFound_ShouldReturnOnlyOneItemAndDoubleQuantity()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        int quantity = request.Quantity * 2;

        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(1);
        cart?.Items[0].Quantity.Should().Be(quantity);
    }

    [Fact, Priority(5)]
    public async Task AddItemQuantityAsync_InvalidItem_ShouldReturnBadRequest()
    {
        AddItemQuantityRequest request = new() { Quantity = 0 };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/AddQuantity/{Guid.Empty}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
        errors.Should().Contain(x => x.Message == "A quantidade do produto não pode ser menor ou igual a zero.");
    }

    [Theory, AutoData, Priority(5)]
    public async Task AddItemQuantityAsync_ItemNotFound_ShouldReturnBadRequest(AddItemQuantityRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/AddQuantity/{request.ProductId}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Item não encontrado.");
    }

    [Fact, Priority(5)]
    public async Task AddItemQuantityAsync_ValidItem_ShouldReturnNoContent()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(new AddItemQuantityRequest 
        { 
            Quantity = 10 
        }), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/AddQuantity/{request.ProductId}", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(6)]
    public async Task GetCartAsync_CartFound_ShouldReturnOnlyOneItemAndDoubleQuantityMoreThen()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        int quantity = (request.Quantity * 2) + 10;

        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(1);
        cart?.Items[0].Quantity.Should().Be(quantity);
    }

    [Fact, Priority(7)]
    public async Task RemoveItemQuantityAsync_InvalidItem_ShouldReturnBadRequest()
    {
        AddItemQuantityRequest request = new() { Quantity = 0 };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/RemoveQuantity/{Guid.Empty}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
        errors.Should().Contain(x => x.Message == "A quantidade do produto não pode ser menor ou igual a zero.");
    }

    [Theory, AutoData, Priority(7)]
    public async Task RemoveItemQuantityAsync_ItemNotFound_ShouldReturnBadRequest(AddItemQuantityRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/RemoveQuantity/{request.ProductId}", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Item não encontrado.");
    }

    [Fact, Priority(7)]
    public async Task RemoveItemQuantityAsync_ValidItem_ShouldReturnNoContent()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(new AddItemQuantityRequest
        {
            Quantity = 20
        }), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync($"api/Carts/Item/RemoveQuantity/{request.ProductId}", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(8)]
    public async Task GetCartAsync_CartFound_ShouldReturnOnlyOneItemAndTripleQuantity()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        int quantity = (request.Quantity * 2) - 10;

        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(1);
        cart?.Items[0].Quantity.Should().Be(quantity);
    }

    [Fact, Priority(9)]
    public async Task DeleteItemAsync_InvalidItem_ShouldReturnBadRequest()
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts/Item/{Guid.Empty}");
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
    }

    [Fact, Priority(9)]
    public async Task DeleteItemAsync_ItemNotFound_ShouldReturnBadRequest()
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts/Item/{Guid.NewGuid()}");
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Item não encontrado.");
    }

    [Fact, Priority(9)]
    public async Task DeleteItemAsync_ValidItem_ShouldReturnNoContent()
    {
        AddItemRequest request = _fixture.GenerateValidAddItemRequestWithTheSameValue();
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts/Item/{request.ProductId}");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(10)]
    public async Task GetCartAsync_CartFoundAfterDeleteItem_ShouldReturnEmptyCart()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(0);
    }

    [Theory, AutoData, Priority(11)]
    public async Task AddItemAsyncToDeleteAllItemsAsync_AddNewItem_ShouldReturnNoContent(AddItemRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/Carts/Item", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory, AutoData, Priority(11)]
    public async Task AddItemAsyncToDeleteAllItemsAsync_AddAnotherItem_ShouldReturnNoContent(AddItemRequest request)
    {
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/Carts/Item", content);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(12)]
    public async Task GetCartAsyncToDeleteAllItemsAsync_CartFound_ShouldReturnOnlyOneItem()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(2);
    }

    [Fact, Priority(13)]
    public async Task DeleteAllItemsAsync_ValidItem_ShouldReturnNoContent()
    {
        HttpResponseMessage response = await _client.DeleteAsync($"api/Carts");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(14)]
    public async Task GetCartAsync_CartFoundAfterDeleteAllItems_ShouldReturnEmptyCart()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/Carts");
        CartDTO? cart = await _helper.DeserializeToObject<CartDTO>(response);

        response.EnsureSuccessStatusCode();
        cart?.Items.Should().HaveCount(0);
    }
}
