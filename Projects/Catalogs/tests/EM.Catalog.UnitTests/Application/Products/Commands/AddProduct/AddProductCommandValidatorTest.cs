using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidatorTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly AddProductCommandValidator _validator;
    private readonly AddProductCommand _addProductCommand;

    public AddProductCommandValidatorTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = _fixture.Freeze<Mock<IReadRepository>>();
        _validator = _fixture.Create<AddProductCommandValidator>();
        _addProductCommand = _fixture.Create<AddProductCommand>();
        Category? category = _fixture.Create<Category?>();

        _repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(category));
    }

    [Fact]
    public async Task Constructor_ValidAddProductCommand_ShouldReturnValidResult()
    {
        ValidationResult result = await _validator.ValidateAsync(_addProductCommand);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Constructor_EmptyAddProductCommandName_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullAddProductCommandName_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyAddProductCommandDescription_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullAddProductCommandDescription_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_ZeroAddProductCommandValue_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_ZeroAddProductCommandQuantity_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductQuantityLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_EmptyAddProductCommandImage_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullAddProductCommandImage_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyAddProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductInvalidCategoryId);
    }

    [Fact]
    public async Task Constructor_DuplicityAddProductCommand_ShouldReturnInvalidResult()
    {
        IEnumerable<Product> products = new List<Product>() { _fixture.Create<Product>() };

        _repositoryMock
            .Setup(x => x.GetProductsByCategoryNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(products));

        ValidationResult result = await _validator.ValidateAsync(_addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductRegisterDuplicity);
    }

    [Fact]
    public async Task Constructor_NotFoundAddProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        Category? category = null;

        _repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(category));

        ValidationResult result = await _validator.ValidateAsync(_addProductCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.ProductCategoryNotFound);
    }
}
