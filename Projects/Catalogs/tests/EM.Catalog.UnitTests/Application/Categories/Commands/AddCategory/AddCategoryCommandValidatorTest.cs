﻿using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;

    public AddCategoryCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCategoryData]
    public async Task Constructor_ValidAddCategoryCommand_ShouldReturnValidResult(
        AddCategoryCommandValidator sut,
        AddCategoryCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_ZeroAddCategoryCommandCode_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryCodeLessThanEqualToZero);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyAddCategoryCommandName_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullAddCategoryCommandName_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyAddCategoryCommandDescription_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullAddCategoryCommandDescription_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_DuplicityAddCategoryCommand_ShouldReturnInvalidResult(
        [Frozen] Mock<ICategoryValidations> validationsMock,
        AddCategoryCommandValidator sut,
        AddCategoryCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<AddCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryRegisterDuplicity);
    }
}
