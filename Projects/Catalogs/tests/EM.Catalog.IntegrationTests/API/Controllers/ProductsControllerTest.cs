using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.API.Models;
using EM.Catalog.Application.Products;
using EM.Catalog.IntegrationTests.Fixtures;
using EM.Catalog.IntegrationTests.Helpers;
using EM.Catalog.IntegrationTests.Models;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using WH.SharedKernel.ResourceManagers;
using Xunit.Priority;

namespace EM.Catalog.IntegrationTests.API.Controllers;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection(nameof(ProductCollection))]
public sealed class ProductsControllerTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly ProductFixture _productFixture;
    private readonly IFixture _fixture;
    private readonly HttpClient _client;

    public ProductsControllerTest(
        IntegrationTestWebAppFactory factory,
        ProductFixture productFixture)
    {
        _productFixture = productFixture;
        _fixture = new Fixture();
        _client = factory.CreateClient();
    }

    private async Task<Guid> GetCategoryIdAsync()
    {
        CategoryRequest request = _productFixture.GenerateValidCategoryRequest();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        AddApiResponse? result = JsonConvert.DeserializeToObject<AddApiResponse>(responseBody);

        return result?.Id ?? Guid.NewGuid();
    }

    [Fact, Priority(0)]
    public async Task AddAsync_ValidProduct_ShouldReturnSuccess()
    {
        //Arrange
        _productFixture.CategoryId = await GetCategoryIdAsync();
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(_productFixture.CategoryId);
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        AddApiResponse? result = JsonConvert.DeserializeToObject<AddApiResponse>(responseBody);
        _productFixture.ProductId = result?.Id;

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(1)]
    public async Task AddAsync_DuplicityProduct_ShouldReturnFailure()
    {
        //Arrange
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(Guid.NewGuid());
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar um produto duplicado.");
    }

    [Fact, Priority(1)]
    public async Task AddAsync_InvalidProduct_ShouldReturnFailure()
    {
        //Arrange
        ProductRequest request = new() { Name = "", Description = "", Value = 0, Quantity = 0, Image = "", CategoryId = Guid.Empty };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O nome do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "O valor do produto não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "A quantidade do produto não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "A imagem do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "O id da categoria do produto não pode ser inválido.");
    }

    [Theory, Priority(1), AutoData]
    public async Task AddAsync_CategoryNotFound_ShouldReturnFailure(ProductRequest request)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "A categoria do produto não foi encontrada.");
    }

    [Fact, Priority(3)]
    public async Task UpdateAsync_ValidProduct_ShouldReturnSuccess()
    {
        //Arrange
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(_productFixture.CategoryId);
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/products/{_productFixture.ProductId}", content);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_DuplicityProduct_ShouldReturnFailure()
    {
        //Arrange
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(_productFixture.CategoryId);
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/products/{Guid.NewGuid()}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar um produto duplicado.");
    }

    [Theory, AutoData, Priority(2)]
    public async Task UpdateAsync_ProductNotFound_ShouldReturnFailure(ProductRequest request)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/products/{Guid.NewGuid()}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Produto não encontrado.");
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_InvalidProduct_ShouldReturnFailure()
    {
        //Arrange
        ProductRequest request = new() { Name = "", Description = "", Value = 0, Quantity = 0, Image = "", CategoryId = Guid.Empty };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/products/{Guid.Empty}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
        errors.Should().Contain(x => x.Message == "O nome do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "O valor do produto não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "A quantidade do produto não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "A imagem do produto não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "O id da categoria do produto não pode ser inválido.");
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_ValidId_ShouldReturnProduct()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{_productFixture.ProductId}");
        string responseBody = await response.Content.ReadAsStringAsync();
        ProductDTO? product = JsonConvert.DeserializeToObject<ProductDTO?>(responseBody);

        //Assert
        product.Should().NotBeNull();
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_InvalidId_ShouldReturnProduct()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_IdNotFound_ShouldReturnProduct()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory, Priority(4)]
    [InlineData("/api/products")]
    [InlineData("/api/products/category/{0}")]
    public async Task GetAsync_ValidRequest_ShouldReturnProducts(string requestUri)
    {
        //Arrange
        requestUri = string.Format(requestUri, _productFixture.CategoryId);

        //Act
        HttpResponseMessage response = await _client.GetAsync(requestUri);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<ProductDTO>? products = JsonConvert.DeserializeToObject<IEnumerable<ProductDTO>>(responseBody);

        //Assert
        products.Should().Contain(x => x.Id == _productFixture.ProductId);
    }

    [Fact, Priority(4)]
    public async Task SearchAsync_ValidRequest_ShouldReturnProducts()
    {
        //Arrange
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(Guid.NewGuid());

        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/search/{request.Name}");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<ProductDTO>? products = JsonConvert.DeserializeToObject<IEnumerable<ProductDTO>>(responseBody);

        //Assert
        products.Should().Contain(x => x.Id == _productFixture.ProductId);
    }

    [Fact, Priority(6)]
    public async Task MakeUnavailableAsync_ValidId_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-unavailable/{_productFixture.ProductId}", null);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(5)]
    public async Task MakeUnavailableAsync_InvalidId_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-unavailable/{Guid.Empty}", null);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
    }

    [Fact, Priority(5)]
    public async Task MakeUnavailableAsync_ProductNotFound_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-unavailable/{Guid.NewGuid()}", null);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Produto não encontrado.");
    }

    [Fact, Priority(7)]
    public async Task UpdateAsync_ProductUnavailable_ShouldReturnSuccess()
    {
        //Arrange
        ProductRequest request = _productFixture.GenerateValidProductRequestWithTheSameValue(_productFixture.CategoryId);
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/products/{_productFixture.ProductId}", content);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(8)]
    public async Task GetByIdAsync_ProductUnavailable_ShouldReturnProductUnavailable()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{_productFixture.ProductId}");
        string responseBody = await response.Content.ReadAsStringAsync();
        ProductDTO? product = JsonConvert.DeserializeToObject<ProductDTO?>(responseBody);

        //Assert
        product?.Available.Should().BeFalse();
    }

    [Fact, Priority(10)]
    public async Task MakeAvailableAsync_ValidId_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-available/{_productFixture.ProductId}", null);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(9)]
    public async Task MakeAvailableAsync_InvalidId_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-available/{Guid.Empty}", null);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
    }

    [Fact, Priority(9)]
    public async Task MakeAvailableAsync_ProductNotFound_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.PatchAsync($"/api/products/make-available/{Guid.NewGuid()}", null);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Produto não encontrado.");
    }

    [Fact, Priority(11)]
    public async Task GetByIdAsync_ProductAvailable_ShouldReturnProductAvailable()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{_productFixture.ProductId}");
        string responseBody = await response.Content.ReadAsStringAsync();
        ProductDTO? product = JsonConvert.DeserializeToObject<ProductDTO?>(responseBody);

        //Assert
        product?.Available.Should().BeTrue();
    }

    [Fact, Priority(13)]
    public async Task DeleteAsync_ValidId_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/products/{_productFixture.ProductId}");

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(12)]
    public async Task DeleteAsync_InvalidId_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/products/{Guid.Empty}");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id do produto não pode ser inválido.");
    }

    [Fact, Priority(12)]
    public async Task DeleteAsync_ProductNotFound_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/products/{Guid.NewGuid()}");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Produto não encontrado.");
    }

    [Fact, Priority(14)]
    public async Task GetByIdAsync_ProductDeleted_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{_productFixture.ProductId}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
