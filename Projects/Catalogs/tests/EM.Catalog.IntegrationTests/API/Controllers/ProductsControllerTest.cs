using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.IntegrationTests.Fixtures;
using EM.Catalog.IntegrationTests.Helpers;
using EM.Catalog.IntegrationTests.Models;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Priority;

namespace EM.Catalog.IntegrationTests.API.Controllers;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection(nameof(ProductCollection))]
public sealed class ProductsControllerTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly ProductFixture _productFixture;
    private readonly IFixture _fixture;
    private readonly HttpClient _client;
    private readonly HttpResponseMessageHelper _helper;
    private readonly Guid _categoryId;

    public ProductsControllerTest(
        IntegrationTestWebAppFactory factory,
        ProductFixture productFixture)
    {
        _productFixture = productFixture;
        _fixture = new Fixture();
        _client = factory.CreateClient();
        _helper = new HttpResponseMessageHelper();
        _categoryId = GetCategoryIdAsync().GetAwaiter().GetResult() ?? Guid.NewGuid();
    }

    private async Task<Guid?> GetCategoryIdAsync()
    {
        HttpResponseMessage response = await _client.GetAsync($"/api/categories");
        IEnumerable<CategoryDTO> categories = await _helper.DeserializeToObject<IEnumerable<CategoryDTO>>(response)
            ?? Enumerable.Empty<CategoryDTO>();

        return categories.FirstOrDefault()?.Id;
    }

    [Fact, Priority(0)]
    public async Task AddAsync_ValidProduct_ShouldReturnSuccess()
    {
        //Arrange
        AddProductCommand command = _productFixture.GenerateValidAddProductCommandWithTheSameValue(_categoryId);
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        AddApiResponse? result = await _helper.DeserializeToObject<AddApiResponse>(response);
        _productFixture.ProductId = result?.Id;

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(1)]
    public async Task AddAsync_DuplicityProduct_ShouldReturnFailure()
    {
        //Arrange
        AddProductCommand command = _productFixture.GenerateValidAddProductCommandWithTheSameValue(Guid.NewGuid());
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar um produto duplicado.");
    }

    [Fact, Priority(1)]
    public async Task AddAsync_InvalidProduct_ShouldReturnFailure()
    {
        //Arrange
        AddProductCommand command = new("", "", 0, 0, "", Guid.Empty);
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

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
    public async Task AddAsync_CategoryNotFound_ShouldReturnFailure(AddProductCommand command)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "A categoria do produto não foi encontrada.");
    }

    [Fact, Priority(3)]
    public async Task UpdateAsync_ValidProduct_ShouldReturnSuccess()
    {
        //Arrange
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Id, _productFixture.ProductId)
            .With(x => x.CategoryId, _categoryId)
            .Create();
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/products", content);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_DuplicityProduct_ShouldReturnFailure()
    {
        //Arrange
        AddProductCommand addCommand = _productFixture.GenerateValidAddProductCommandWithTheSameValue(_categoryId);
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, addCommand.Name)
            .Create();
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar um produto duplicado.");
    }

    [Theory, AutoData, Priority(2)]
    public async Task UpdateAsync_ProductNotFound_ShouldReturnFailure(UpdateProductCommand command)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Produto não encontrado.");
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_InvalidProduct_ShouldReturnFailure()
    {
        //Arrange
        UpdateProductCommand command = new(Guid.Empty, "", "", 0, 0, "", false, Guid.Empty);
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/products", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

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
        ProductDTO? product = await _helper.DeserializeToObject<ProductDTO?>(response);

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
    public async Task GetAsync_ValidRequest_ShouldReturnProduct(string requestUri)
    {
        //Arrange
        requestUri = string.Format(requestUri, _categoryId);

        //Act
        HttpResponseMessage response = await _client.GetAsync(requestUri);
        IEnumerable<ProductDTO> products = await _helper.DeserializeToObject<IEnumerable<ProductDTO>>(response)
            ?? Enumerable.Empty<ProductDTO>();

        //Assert
        products.Should().Contain(x => x.Id == _productFixture.ProductId);
    }
}
