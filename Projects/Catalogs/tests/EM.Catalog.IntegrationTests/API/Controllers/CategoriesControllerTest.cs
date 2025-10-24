using AutoFixture.Xunit2;
using EM.Catalog.API.Models;
using EM.Catalog.Application.Categories;
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
[Collection(nameof(CategoryCollection))]
public sealed class CategoriesControllerTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly CategoryFixture _categoryFixture;
    private readonly HttpClient _client;

    public CategoriesControllerTest(
        IntegrationTestWebAppFactory factory,
        CategoryFixture categoryFixture)
    {
        _categoryFixture = categoryFixture;
        _client = factory.CreateClient();
    }

    [Fact, Priority(0)]
    public async Task AddAsync_ValidCategory_ShouldReturnSuccess()
    {
        //Arrange
        CategoryRequest request = _categoryFixture.GenerateValidCategoryRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        AddApiResponse? result = JsonConvert.DeserializeToObject<AddApiResponse>(responseBody);
        _categoryFixture.CategoryId = result?.Id;

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(1)]
    public async Task AddAsync_DuplicityCategory_ShouldReturnFailure()
    {
        //Arrange
        CategoryRequest request = _categoryFixture.GenerateValidCategoryRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar uma categoria duplicada.");
    }

    [Fact, Priority(1)]
    public async Task AddAsync_InvalidCategory_ShouldReturnFailure()
    {
        //Arrange
        CategoryRequest request = new() { Code = 0, Name = "", Description = "" };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O código da categoria não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "O nome da categoria não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição da categoria não pode ser vazio ou nulo.");
    }

    [Theory, AutoData, Priority(3)]
    public async Task UpdateAsync_ValidCategory_ShouldReturnSuccess(CategoryRequest request)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/categories/{_categoryFixture.CategoryId}", content);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_DuplicityCategory_ShouldReturnFailure()
    {
        //Arrange
        CategoryRequest request = _categoryFixture.GenerateValidCategoryRequestWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/categories/{Guid.NewGuid()}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar uma categoria duplicada.");
    }

    [Theory, AutoData, Priority(2)]
    public async Task UpdateAsync_CategoryNotFound_ShouldReturnFailure(CategoryRequest request)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/categories/{Guid.NewGuid()}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Categoria não encontrada.");
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_InvalidCategory_ShouldReturnFailure()
    {
        //Arrange
        CategoryRequest request = new() { Code = 0, Name = "", Description = "" };
        StringContent content = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync($"/api/categories/{Guid.Empty}", content);
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id da categoria não pode ser inválido.");
        errors.Should().Contain(x => x.Message == "O código da categoria não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "O nome da categoria não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição da categoria não pode ser vazio ou nulo.");
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_ValidId_ShouldReturnCategory()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{_categoryFixture.CategoryId}");
        string responseBody = await response.Content.ReadAsStringAsync();
        CategoryDTO? category = JsonConvert.DeserializeToObject<CategoryDTO>(responseBody);

        //Assert
        category.Should().NotBeNull();
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_InvalidId_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(4)]
    public async Task GetByIdAsync_CategoryNotFound_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(4)]
    public async Task GetAllAsync_ValidRequest_ShouldReturnCategories()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<CategoryDTO>? categories = JsonConvert.DeserializeToObject<IEnumerable<CategoryDTO>>(responseBody);

        //Assert
        categories.Should().Contain(x => x.Id == _categoryFixture.CategoryId);
    }

    [Fact, Priority(6)]
    public async Task DeleteAsync_ValidId_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/categories/{_categoryFixture.CategoryId}");

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(5)]
    public async Task DeleteAsync_InvalidId_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/categories/{Guid.Empty}");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id da categoria não pode ser inválido.");
    }

    [Fact, Priority(5)]
    public async Task DeleteAsync_CategoryNotFound_ShouldReturnFailure()
    {
        //Act
        HttpResponseMessage response = await _client.DeleteAsync($"/api/categories/{Guid.NewGuid()}");
        string responseBody = await response.Content.ReadAsStringAsync();
        IEnumerable<Error>? errors = JsonConvert.DeserializeToObject<IEnumerable<Error>>(responseBody);

        //Assert
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Categoria não encontrada.");
    }

    [Fact, Priority(7)]
    public async Task GetByIdAsync_CategoryDeleted_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{_categoryFixture.CategoryId}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
