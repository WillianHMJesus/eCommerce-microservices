using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.Domain;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _writeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;
    private readonly UpdateProductCommand _updateProductCommand;

    public UpdateProductCommandHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _writeRepositoryMock = fixture.Freeze<Mock<IWriteRepository>>();
        _unitOfWorkMock = fixture.Freeze<Mock<IUnitOfWork>>();
        _mediatorMock = fixture.Freeze<Mock<IMediator>>();
        Product product = fixture.Create<Product>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<UpdateProductCommand>()))
            .Returns(product);

        _updateProductCommandHandler = fixture.Create<UpdateProductCommandHandler>();
        _updateProductCommand = fixture.Create<UpdateProductCommand>();
    }

    [Fact]
    public async Task Handle_ValidUpdateProductCommand_ShouldInvokeWriteRepositoryUpdateProductAsync()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        Result result = await _updateProductCommandHandler.Handle(_updateProductCommand, CancellationToken.None);

        _writeRepositoryMock.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        _mediatorMock.Verify(x => x.Publish(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ProductCategoryNotFound_ShouldReturnWithFailed()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));

        DomainException domainException = await Assert.ThrowsAsync<DomainException>(
            async () => await _updateProductCommandHandler.Handle(_updateProductCommand, CancellationToken.None));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductAnErrorOccorred);
    }
}
