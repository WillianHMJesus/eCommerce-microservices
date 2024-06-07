using AutoFixture;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.IntegrationTests.Fixture;
using EM.Catalog.IntegrationTests.Helpers;
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
[Collection(nameof(AddCategoryCommandCollection))]
public sealed class CategoriesControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly AddCategoryCommandFixture _fixture;
    private readonly HttpResponseMessageHelper _helper;

    public CategoriesControllerTest(
        WebApplicationFactory<Program> factory, 
        AddCategoryCommandFixture fixture)
    {
        _factory = factory;
        _fixture = fixture;
        _helper = new HttpResponseMessageHelper();
    }

    [Fact, Priority(0)]
    public async Task AddAsync_ValidCategory_ShouldReturnSuccess()
    {
        //Arrange
        HttpClient client = _factory.CreateClient();
        AddCategoryCommand addCategoryCommand = _fixture.GenerateValidAddCategoryCommandWithTheSameValue();
        StringContent httpContent = new(JsonSerializer.Serialize(addCategoryCommand), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await client.PostAsync("/api/categories", httpContent);
        Guid? categoryId = await _helper.DeserializeToObject<Guid>(response);
        _fixture.CategoryId = categoryId;

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, Priority(1)]
    public async Task AddAsync_DuplicityCategory_ShouldReturnFailure()
    {
        //Arrange
        HttpClient client = _factory.CreateClient();
        AddCategoryCommand addCategoryCommand = _fixture.GenerateValidAddCategoryCommandWithTheSameValue();
        StringContent httpContent = new(JsonSerializer.Serialize(addCategoryCommand), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await client.PostAsync("/api/categories", httpContent);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "Não é possível cadastrar ou atualizar uma categoria duplicada.");
    }

    [Fact, Priority(1)]
    public async Task AddAsync_InvalidCategory_ShouldReturnFailure()
    {
        //Arrange
        HttpClient client = _factory.CreateClient();
        AddCategoryCommand addCategoryCommand = new(0, "", "");
        StringContent httpContent = new(JsonSerializer.Serialize(addCategoryCommand), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await client.PostAsync("/api/categories", httpContent);
        IEnumerable<Error>? errors = await _helper.DeserializeToObject<IEnumerable<Error>>(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == "O código da categoria não pode ser menor ou igual a zero.");
        errors.Should().Contain(x => x.Message == "O nome da categoria não pode ser vazio ou nulo.");
        errors.Should().Contain(x => x.Message == "A descrição da categoria não pode ser vazio ou nulo.");
    }
}
