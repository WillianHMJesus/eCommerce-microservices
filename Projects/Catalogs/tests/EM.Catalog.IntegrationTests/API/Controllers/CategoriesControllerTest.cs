using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.IntegrationTests.Fixtures;
using EM.Catalog.IntegrationTests.Helpers;
using EM.Catalog.IntegrationTests.Models;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Priority;

namespace EM.Catalog.IntegrationTests.API.Controllers;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection(nameof(CategoryCollection))]
public sealed class CategoriesControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly CategoryFixture _categoryFixture;
    private readonly IFixture _fixture;
    private readonly HttpClient _client;
    private readonly HttpResponseMessageHelper _helper;

    public CategoriesControllerTest(
        WebApplicationFactory<Program> factory, 
        CategoryFixture categoryFixture)
    {
        _categoryFixture = categoryFixture;
        _fixture = new Fixture();
        _client = factory.CreateClient();
        _helper = new HttpResponseMessageHelper();
    }

    [Fact, Priority(0)]
    public async Task AddAsync_ValidCategory_ShouldReturnSuccess()
    {
        //Arrange
        AddCategoryCommand command = _categoryFixture.GenerateValidAddCategoryCommandWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        AddApiResponse? result = await _helper.DeserializeToObject<AddApiResponse>(response);
        _categoryFixture.CategoryId = result?.Id;

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(1)]
    public async Task AddAsync_DuplicityCategory_ShouldReturnFailure()
    {
        //Arrange
        AddCategoryCommand command = _categoryFixture.GenerateValidAddCategoryCommandWithTheSameValue();
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar uma categoria duplicada.");
    }

    [Fact, Priority(1)]
    public async Task AddAsync_InvalidCategory_ShouldReturnFailure()
    {
        //Arrange
        AddCategoryCommand command = new(0, "", "");
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/categories", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O código da categoria não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "O nome da categoria não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição da categoria não pode ser vazio ou nulo.");
    }

    [Fact, Priority(3)]
    public async Task UpdateAsync_ValidCategory_ShouldReturnSuccess()
    {
        //Arrange
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, _categoryFixture.CategoryId)
            .Create();
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/categories", content);

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_DuplicityCategory_ShouldReturnFailure()
    {
        //Arrange
        AddCategoryCommand addCommand = _categoryFixture.GenerateValidAddCategoryCommandWithTheSameValue();
        UpdateCategoryCommand command = new(Guid.NewGuid(), addCommand.Code, addCommand.Name, addCommand.Description);
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/categories", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar uma categoria duplicada.");
    }

    [Theory, AutoData, Priority(2)]
    public async Task UpdateAsync_CategoryNotFound_ShouldReturnFailure(UpdateCategoryCommand command)
    {
        //Arrange
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/categories", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O id da categoria não pode ser inválido.");
    }

    [Fact, Priority(2)]
    public async Task UpdateAsync_InvalidCategory_ShouldReturnFailure()
    {
        //Arrange
        UpdateCategoryCommand command = new(Guid.Empty, 0, "", "");
        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/categories", content);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

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
        CategoryDTO? category = await _helper.DeserializeToObject<CategoryDTO> (response);

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
    public async Task GetByIdAsync_IdNotFound_ShouldReturnNoContent()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact, Priority(4)]
    public async Task GetAllAsync_ValidRequest_ShouldReturnCategory()
    {
        //Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories");
        IEnumerable<CategoryDTO> categories = await _helper.DeserializeToObject<IEnumerable<CategoryDTO>>(response) 
            ?? Enumerable.Empty<CategoryDTO>();

        //Assert
        categories.Should().Contain(x => x.Id == _categoryFixture.CategoryId);
    }
}
