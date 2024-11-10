using AutoFixture.Xunit2;
using AutoFixture;
using EM.Catalog.Application.Products.Validations;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;
using EM.Catalog.Application.Products.Commands.MakeUnavailableProduct;
using FluentValidation.Results;
using FluentAssertions;
using EM.Common.Core.ResourceManagers;

namespace EM.Catalog.UnitTests.Application.Products.Commands.MakeUnavailableProduct;

public sealed class MakeUnavailableProductCommandValidatorTest
{
    private readonly IFixture _fixture;

    public MakeUnavailableProductCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoProductData]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult(
        MakeUnavailableProductCommandValidator sut,
        MakeUnavailableProductCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    public async Task Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult(
        MakeUnavailableProductCommandValidator sut)
    {
        MakeUnavailableProductCommand command = _fixture.Build<MakeUnavailableProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoProductData]
    public async Task Constructor_UpdateProductCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductValidations> validationsMock,
        MakeUnavailableProductCommandValidator sut,
        MakeUnavailableProductCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductNotFound);
    }
}
