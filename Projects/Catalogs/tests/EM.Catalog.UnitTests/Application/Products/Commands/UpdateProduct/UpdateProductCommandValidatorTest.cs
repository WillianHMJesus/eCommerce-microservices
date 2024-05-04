using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidatorTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly UpdateProductCommandValidator _validator;
    private readonly UpdateProductCommand _updateProductCommand;

    public UpdateProductCommandValidatorTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = _fixture.Freeze<Mock<IReadRepository>>();
        _validator = _fixture.Create<UpdateProductCommandValidator>();
        _updateProductCommand = _fixture.Create<UpdateProductCommand>();
        Category? category = _fixture.Create<Category?>();

        _repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(category));
    }

    [Fact]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult()
    {
        ValidationResult result = await _validator.ValidateAsync(_updateProductCommand);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductInvalidId);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateProductCommandName_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullUpdateProductCommandName_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateProductCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullUpdateProductCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_ZeroUpdateProductCommandValue_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_ZeroUpdateProductCommandQuantity_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductQuantityLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateProductCommandImage_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullUpdateProductCommandImage_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductInvalidCategoryId);
    }

    [Fact]
    public async Task Constructor_DuplicityUpdateProductCommand_ShouldReturnInvalidResult()
    {
        IEnumerable<Product> products = new List<Product>() { _fixture.Create<Product>() };

        _repositoryMock
            .Setup(x => x.GetProductsByCategoryNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .Returns(Task.FromResult(products));

        ValidationResult result = await _validator.ValidateAsync(_updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductRegisterDuplicity);
    }

    [Fact]
    public async Task Constructor_NotFoundUpdateProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        Category? category = null;

        _repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .Returns(Task.FromResult(category));

        ValidationResult result = await _validator.ValidateAsync(_updateProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductCategoryNotFound);
    }
}
