using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidatorTest
{
    private readonly IFixture _fixture;

    public AddProductCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoProductData]
    public async Task Constructor_ValidAddProductCommand_ShouldReturnValidResult(
        [Frozen] Mock<IReadRepository> repositoryMock,
        AddProductCommandValidator sut,
        AddProductCommand command)
    {
        repositoryMock
            .Setup(x => x.GetProductsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyAddProductCommandName_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNameNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullAddProductCommandName_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNameNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyAddProductCommandDescription_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductDescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullAddProductCommandDescription_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductDescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_ZeroAddProductCommandValue_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductValueLessThanEqualToZero);
    }

    [Theory, AutoProductData]
    public async Task Constructor_ZeroAddProductCommandQuantity_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyAddProductCommandImage_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullAddProductCommandImage_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyAddProductCommandCategoryId_ShouldReturnInvalidResult(
        AddProductCommandValidator sut)
    {
        AddProductCommand command = _fixture.Build<AddProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidCategoryId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_DuplicityAddProductCommand_ShouldReturnInvalidResult(
        AddProductCommandValidator sut,
        AddProductCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductRegisterDuplicity);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NotFoundAddProductCommandCategoryId_ShouldReturnInvalidResult(
        [Frozen] Mock<IReadRepository> repositoryMock,
        AddProductCommandValidator sut,
        AddProductCommand command)
    {
        Category? category = null;

        repositoryMock
            .Setup(x => x.GetProductsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductCategoryNotFound);
    }
}
