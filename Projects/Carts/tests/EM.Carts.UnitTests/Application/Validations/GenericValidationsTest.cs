using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.Application.Validations;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.Validations;

public sealed class GenericValidationsTest
{
    [Theory, AutoCartData]
    public async Task ValidateCartByUserIdAsync_CartFound_ShouldReturnTrue(
        GenericValidations sut)
    {
        bool result = await sut.ValidateCartByUserIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoCartData]
    public async Task ValidateCartByUserIdAsync_CartNotFound_ShouldReturnFalse(
        [Frozen] Mock<ICartRepository> repositoryMock,
        GenericValidations sut)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Cart);

        bool result = await sut.ValidateCartByUserIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCartData]
    public async Task ValidateItemByProductIdAsync_ItemFound_ShouldReturnTrue(
        [Frozen] Mock<ICartRepository> repositoryMock,
        GenericValidations sut,
        Cart cart,
        Item item)
    {
        cart.AddItem(item);

        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        bool result = await sut.ValidateItemByProductIdAsync(item.ProductId, cart.UserId, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoCartData]
    public async Task ValidateItemByProductIdAsync_ItemNotFound_ShouldReturnFalse(
        GenericValidations sut)
    {
        bool result = await sut.ValidateItemByProductIdAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCartData]
    public async Task ValidateItemByProductIdAsync_CartNotFound_ShouldReturnFalse(
        [Frozen] Mock<ICartRepository> repositoryMock,
        GenericValidations sut)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Cart);

        bool result = await sut.ValidateItemByProductIdAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCartData]
    public async Task ValidateProductAvailabilityAsync_NullProduct_ShouldReturnFalse(
        [Frozen] Mock<ICatalogExternalService> externalServiceMock,
        GenericValidations sut)
    {
        externalServiceMock
            .Setup(x => x.GetProductsByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ProductDTO);

        bool result = await sut.ValidateProductAvailabilityAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCartData]
    public async Task ValidateProductAvailabilityAsync_ProductUnavailable_ShouldReturnFalse(
        [Frozen] Mock<ICatalogExternalService> externalServiceMock,
        GenericValidations sut)
    {
        ProductDTO productDTO = new Fixture().Build<ProductDTO>()
            .With(x => x.Available, false)
            .With(x => x.Quantity, 0)
            .Create();

        externalServiceMock
            .Setup(x => x.GetProductsByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productDTO);

        bool result = await sut.ValidateProductAvailabilityAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }
}
