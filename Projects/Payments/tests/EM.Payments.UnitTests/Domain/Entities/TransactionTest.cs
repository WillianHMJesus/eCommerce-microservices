using AutoFixture;
using AutoFixture.Xunit2;
using EM.Common.Core.ResourceManagers;
using EM.Payments.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace EM.Payments.UnitTests.Domain.Entities;

public sealed class TransactionTest
{
    private readonly Fixture _fixture;

    public TransactionTest() => _fixture = new();

    [Theory, AutoData]
    public void Validate_ValidTransaction_ShouldNotReturnDomainException(Transaction transaction)
    {
        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_DefaultUserId_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.UserIdInvalid);
    }

    [Fact]
    public void Validate_DefaultOrderId_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.OrderId, Guid.Empty)
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.OrderIdInvalid);
    }

    [Fact]
    public void Validate_ZeroValue_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.Value, 0)
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ValueInvalid);
    }

    [Fact]
    public void Validate_NullCardNumber_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.CardNumber, null as string)
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.CardNumberInvalid);
    }

    [Fact]
    public void Validate_EmptyCardNumber_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.CardNumber, "")
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.CardNumberInvalid);
    }

    [Fact]
    public void Validate_NullStatus_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.Status, null as string)
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.StatusInvalid);
    }

    [Fact]
    public void Validate_EmptyStatus_ShouldReturnDomainException()
    {
        Transaction transaction = _fixture.Build<Transaction>()
            .With(x => x.Status, "")
            .Create();

        Exception domainException = Record.Exception(() => transaction.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.StatusInvalid);
    }
}
