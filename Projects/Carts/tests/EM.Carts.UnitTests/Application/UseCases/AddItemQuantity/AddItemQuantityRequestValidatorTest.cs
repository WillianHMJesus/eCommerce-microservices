using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.Validations;
using EM.Carts.UnitTests.Application.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.AddItemQuantity;

public sealed class AddItemQuantityRequestValidatorTest
{
    private readonly IFixture _fixture;

    public AddItemQuantityRequestValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCartData]
    public async Task Constructor_ValidAddItemQuantity_ShouldReturnValidResult(
        AddItemQuantityRequestValidator sut,
        AddItemQuantityRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        AddItemQuantityRequestValidator sut)
    {
        AddItemQuantityRequest request = _fixture.Build<AddItemQuantityRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }

    [Theory,  AutoCartData]
    public async Task Constructor_DefaultProductId_ShouldReturnInvalidResult(
        AddItemQuantityRequestValidator sut)
    {
        AddItemQuantityRequest request = _fixture.Build<AddItemQuantityRequest>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoCartData]
    public async Task Constructor_ZeroQuantity_ShouldReturnInvalidResult(
        AddItemQuantityRequestValidator sut)
    {
        AddItemQuantityRequest request = _fixture.Build<AddItemQuantityRequest>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_NegativeQuantity_ShouldReturnInvalidResult(
        AddItemQuantityRequestValidator sut)
    {
        AddItemQuantityRequest request = _fixture.Build<AddItemQuantityRequest>()
            .With(x => x.Quantity, -1)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_CartNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        AddItemQuantityRequestValidator sut,
        AddItemQuantityRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CartNotFound);
    }

    [Theory, AutoCartData]
    public async Task Constructor_ItemNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        AddItemQuantityRequestValidator sut,
        AddItemQuantityRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateItemByProductIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ItemNotFound);
    }
}
