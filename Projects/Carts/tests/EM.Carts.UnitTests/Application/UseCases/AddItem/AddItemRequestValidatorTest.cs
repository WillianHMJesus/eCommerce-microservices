using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.UseCases.AddItem;
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

namespace EM.Carts.UnitTests.Application.UseCases.AddItem;

public sealed class AddItemRequestValidatorTest
{
    private readonly IFixture _fixture;

    public AddItemRequestValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCartData]
    public async Task Constructor_ValidAddItem_ShouldReturnValidResult(
        AddItemRequestValidator sut,
        AddItemRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        AddItemRequestValidator sut)
    {
        AddItemRequest request = _fixture.Build<AddItemRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultProductId_ShouldReturnInvalidResult(
        AddItemRequestValidator sut)
    {
        AddItemRequest request = _fixture.Build<AddItemRequest>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoCartData]
    public async Task Constructor_ZeroQuantity_ShouldReturnInvalidResult(
        AddItemRequestValidator sut)
    {
        AddItemRequest request = _fixture.Build<AddItemRequest>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_NegativeQuantity_ShouldReturnInvalidResult(
        AddItemRequestValidator sut)
    {
        AddItemRequest request = _fixture.Build<AddItemRequest>()
            .With(x => x.Quantity, -1)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_ProductUnavailable_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        AddItemRequestValidator sut,
        AddItemRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateProductAvailabilityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductUnavailable);
    }
}
