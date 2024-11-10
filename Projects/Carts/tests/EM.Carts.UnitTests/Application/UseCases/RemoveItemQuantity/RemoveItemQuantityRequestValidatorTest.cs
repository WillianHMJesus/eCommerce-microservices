﻿using AutoFixture.Xunit2;
using AutoFixture;
using EM.Carts.Application.UseCases.RemoveItemQuantity;
using EM.Carts.Application.Validations;
using EM.Carts.UnitTests.Application.CustomAutoData;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.Results;
using FluentAssertions;
using EM.Common.Core.ResourceManagers;
using System.Threading;

namespace EM.Carts.UnitTests.Application.UseCases.RemoveItemQuantity;

public sealed class RemoveItemQuantityRequestValidatorTest
{
    private readonly IFixture _fixture;

    public RemoveItemQuantityRequestValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCartData]
    public async Task Constructor_ValidRemoveItemQuantity_ShouldReturnValidResult(
        RemoveItemQuantityRequestValidator sut,
        RemoveItemQuantityRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        RemoveItemQuantityRequestValidator sut)
    {
        RemoveItemQuantityRequest request = _fixture.Build<RemoveItemQuantityRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultProductId_ShouldReturnInvalidResult(
        RemoveItemQuantityRequestValidator sut)
    {
        RemoveItemQuantityRequest request = _fixture.Build<RemoveItemQuantityRequest>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoCartData]
    public async Task Constructor_ZeroQuantity_ShouldReturnInvalidResult(
        RemoveItemQuantityRequestValidator sut)
    {
        RemoveItemQuantityRequest request = _fixture.Build<RemoveItemQuantityRequest>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_NegativeQuantity_ShouldReturnInvalidResult(
        RemoveItemQuantityRequestValidator sut)
    {
        RemoveItemQuantityRequest request = _fixture.Build<RemoveItemQuantityRequest>()
            .With(x => x.Quantity, -1)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoCartData]
    public async Task Constructor_CartNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        RemoveItemQuantityRequestValidator sut,
        RemoveItemQuantityRequest request)
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
        RemoveItemQuantityRequestValidator sut,
        RemoveItemQuantityRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateItemByProductIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ItemNotFound);
    }
}
