using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidatorTest
{
    private readonly IFixture _fixture;

    public UpdateProductCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoProductData]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult(
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandName_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNameNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullUpdateProductCommandName_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNameNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandDescription_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductDescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullUpdateProductCommandDescription_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductDescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_ZeroUpdateProductCommandValue_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductValueLessThanEqualToZero);
    }

    [Theory, AutoProductData]
    public async Task Constructor_ZeroUpdateProductCommandQuantity_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandImage_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NullUpdateProductCommandImage_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandCategoryId_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        UpdateProductCommand command = _fixture.Build<UpdateProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidCategoryId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_UpdateProductCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductValidations> validationsMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNotFound);
    }

    [Theory, AutoProductData]
    public async Task Constructor_DuplicityUpdateProductCommand_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductValidations> validationsMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductRegisterDuplicity);
    }

    [Theory, AutoProductData]
    public async Task Constructor_NotFoundUpdateProductCommandCategoryId_ShouldReturnInvalidResult(
        [Frozen] Mock<ICategoryValidations> validationsMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductCategoryNotFound);
    }
}
