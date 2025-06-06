﻿using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.UseCases.DeleteAllItems;
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

namespace EM.Carts.UnitTests.Application.UseCases.DeleteAllItems;

public sealed class DeleteAllItemsRequestValidatorTest
{
    [Theory, AutoCartData]
    public async Task Constructor_ValidDeleteAllItems_ShouldReturnValidResult(
        DeleteAllItemsRequestValidator sut,
        DeleteAllItemsRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        DeleteAllItemsRequestValidator sut)
    {
        DeleteAllItemsRequest request = new Fixture().Build<DeleteAllItemsRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }

    [Theory, AutoCartData]
    public async Task Constructor_CartNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        DeleteAllItemsRequestValidator sut,
        DeleteAllItemsRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CartNotFound);
    }
}
