using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.Validations;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using Moq;
using System;
using System.Threading;

namespace EM.Carts.UnitTests.Application.CustomAutoData;

public class AutoCartDataAttribute : AutoDataAttribute
{
    public AutoCartDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        Cart cart = fixture.Create<Cart>();
        Item item = fixture.Create<Item>();
        cart.AddItem(item);

        fixture.Customize<Item>(x => x.With(x => x.ProductId, item.ProductId));

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Item>(It.IsAny<object>()))
            .Returns(item);

        fixture.Freeze<Mock<ICartRepository>>()
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        fixture.Freeze<Mock<IGenericValidations>>()
            .Setup(x => x.ValidateCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IGenericValidations>>()
            .Setup(x => x.ValidateItemByProductIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IGenericValidations>>()
            .Setup(x => x.ValidateProductAvailabilityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        return fixture;
    }
}
