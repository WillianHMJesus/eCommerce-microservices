using AutoFixture.Xunit2;
using AutoFixture;
using EM.Catalog.Application.Products.Validations;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;
using EM.Catalog.Application.Products.Commands.MakeAvailableProduct;
using FluentValidation.Results;
using FluentAssertions;
using EM.Common.Core.ResourceManagers;

namespace EM.Catalog.UnitTests.Application.Products.Commands.MakeAvailableProduct;

public sealed class MakeAvailableProductCommandValidatorTest
{
    private readonly IFixture _fixture;

    public MakeAvailableProductCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoProductData]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult(
        MakeAvailableProductCommandValidator sut,
        MakeAvailableProductCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult(
        MakeAvailableProductCommandValidator sut)
    {
        MakeAvailableProductCommand command = _fixture.Build<MakeAvailableProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_UpdateProductCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductValidations> validationsMock,
        MakeAvailableProductCommandValidator sut,
        MakeAvailableProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNotFound);
    }
}
