using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.UseCases.DeleteItem;
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

namespace EM.Carts.UnitTests.Application.UseCases.DeleteItem;

public sealed class DeleteItemRequestValidatorTest
{
    private readonly IFixture _fixture;

    public DeleteItemRequestValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCartData]
    public async Task Constructor_ValidDeleteItem_ShouldReturnValidResult(
        DeleteItemRequestValidator sut,
        DeleteItemRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        DeleteItemRequestValidator sut)
    {
        DeleteItemRequest request = _fixture.Build<DeleteItemRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultProductId_ShouldReturnInvalidResult(
        DeleteItemRequestValidator sut)
    {
        DeleteItemRequest request = _fixture.Build<DeleteItemRequest>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ProductInvalidId);
    }

    [Theory, AutoCartData]
    public async Task Constructor_CartNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IGenericValidations> validationsMock,
        DeleteItemRequestValidator sut,
        DeleteItemRequest request)
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
        DeleteItemRequestValidator sut,
        DeleteItemRequest request)
    {
        validationsMock
            .Setup(x => x.ValidateItemByProductIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.ItemNotFound);
    }
}
