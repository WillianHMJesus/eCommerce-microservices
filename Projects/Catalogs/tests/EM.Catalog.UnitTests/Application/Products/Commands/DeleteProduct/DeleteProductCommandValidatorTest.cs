using AutoFixture;
using EM.Catalog.UnitTests.CustomAutoData;
using Xunit;
using FluentValidation.Results;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using FluentAssertions;
using AutoFixture.Xunit2;
using EM.Common.Core.ResourceManagers;
using Moq;
using EM.Catalog.Application.Products.Validations;

namespace EM.Catalog.UnitTests.Application.Products.Commands.DeleteProduct;
public sealed class DeleteProductCommandValidatorTest
{
    private readonly IFixture _fixture;

    public DeleteProductCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoProductData]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult(
        DeleteProductCommandValidator sut,
        DeleteProductCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult(
        DeleteProductCommandValidator sut)
    {
        DeleteProductCommand command = _fixture.Build<DeleteProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_UpdateProductCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductValidations> validationsMock,
        DeleteProductCommandValidator sut,
        DeleteProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNotFound);
    }
}
