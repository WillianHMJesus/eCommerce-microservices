using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _writeRepositoryMock;
    private readonly Mock<IReadRepository> _readRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;

    public UpdateProductCommandHandlerTest()
    {
        _writeRepositoryMock = new();
        _readRepositoryMock = new();
        _unitOfWorkMock = new();
        _mediatorMock = new();
        _mapperMock = new();
        _updateProductCommandHandler = new(_writeRepositoryMock.Object, _readRepositoryMock.Object, _unitOfWorkMock.Object, _mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidUpdateProductCommand_ShouldInvokeWriteRepositoryUpdateProductAsync()
    {
        Category? category = new Fixture().Create<Category>();
        _readRepositoryMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Category?>(category));
        Product product = new Fixture().Create<Product>();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<UpdateProductCommand>())).Returns(product);

        Result result = await _updateProductCommandHandler.Handle(new Fixture().Create<UpdateProductCommand>(), It.IsAny<CancellationToken>());

        _writeRepositoryMock.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_ProductCategoryNotFound_ShouldReturnWithFailed()
    {
        Result result = await _updateProductCommandHandler.Handle(new Fixture().Create<UpdateProductCommand>(), It.IsAny<CancellationToken>());

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
