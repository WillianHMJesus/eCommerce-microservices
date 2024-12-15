using AutoFixture;
using AutoFixture.Xunit2;
using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Entities.Enums;
using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System;
using Xunit;

namespace EM.Checkout.UnitTests.Domain.Entities;

public sealed class OrderTest
{
    private readonly Fixture _fixture;

    public OrderTest() => _fixture = new();

    [Theory, AutoData]
    public void Validate_ValidOrder_ShouldNotReturnDomainException(Order order)
    {
        Exception domainException = Record.Exception(() => order.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_EmptyOrderUserId_ShouldReturnDomainException()
    {
        Order order = _fixture.Build<Order>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => order.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.UserIdInvalid);
    }

    [Fact]
    public void Validate_EmptyOrderNumber_ShouldReturnDomainException()
    {
        Order order = _fixture.Build<Order>()
            .With(x => x.Number, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => order.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.OrderNumberNull);
    }

    [Fact]
    public void Validate_NullOrderNumber_ShouldReturnDomainException()
    {
        Order order = _fixture.Build<Order>()
            .With(x => x.Number, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => order.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.OrderNumberNull);
    }

    [Theory, AutoData]
    public void AddItem_ValidOrderItem_ShouldAddItemInOrder(Order order, Item item)
    {
        order.AddItem(item);

        order.Items.Should().Contain(item);
    }

    [Theory, AutoData]
    public void AddItem_NullOrderItem_ShouldReturnDomainException(Order order)
    {
#pragma warning disable 8625
        DomainException domainException = Assert.Throws<DomainException>(() => order.AddItem(null));
#pragma warning restore 8625

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.OrderItemNull);
    }

    [Theory, AutoData]
    public void PayOrder_ValidOrder_ShouldChangeOrderStatusToPaid(Order order)
    {
        order.PayOrder();

        order.OrderStatus.Should().Be(OrderStatus.Paid);
    }

    [Theory, AutoData]
    public void RefuseOrder_ValidOrder_ShouldChangeOrderStatusToPaymentRefused(Order order)
    {
        order.RefuseOrder();

        order.OrderStatus.Should().Be(OrderStatus.PaymentRefused);
    }
}
