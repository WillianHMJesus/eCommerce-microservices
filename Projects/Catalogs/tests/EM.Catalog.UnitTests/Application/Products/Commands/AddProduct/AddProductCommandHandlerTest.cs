using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Shared.Core;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _writeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AddProductCommandHandler _addProductCommandHandler;
    private readonly AddProductCommand _addProductCommand;
    private readonly Product _product;

    public AddProductCommandHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _writeRepositoryMock = fixture.Freeze<Mock<IWriteRepository>>();
        _unitOfWorkMock = fixture.Freeze<Mock<IUnitOfWork>>();
        _mediatorMock = fixture.Freeze<Mock<IMediator>>();
        _product = fixture.Create<Product>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
            .Returns(_product);

        _addProductCommandHandler = fixture.Create<AddProductCommandHandler>();
        _addProductCommand = fixture.Create<AddProductCommand>();
    }

    [Fact]
    public async Task Handle_ValidCommit_ShouldReturnWithSuccess()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        Result result = await _addProductCommandHandler.Handle(_addProductCommand, CancellationToken.None);

        _writeRepositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        _mediatorMock.Verify(x => x.Publish(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(_product.Id);
    }

    [Fact]
    public async Task Handle_ProductCategoryNotFound_ShouldReturnWithFailed()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));

        DomainException domainException = await Assert.ThrowsAsync<DomainException>(
            async () => await _addProductCommandHandler.Handle(_addProductCommand, CancellationToken.None));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductAnErrorOccorred);
    }
}
