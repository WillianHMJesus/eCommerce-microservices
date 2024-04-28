using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _writeRepositoryMock;
    private readonly Mock<IReadRepository> _readRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddProductCommandHandler _addProductCommandHandler;

    public AddProductCommandHandlerTest()
    {
        _writeRepositoryMock = new();
        _readRepositoryMock = new();
        _unitOfWorkMock = new();
        _mediatorMock = new();
        _mapperMock = new();
        _addProductCommandHandler = new(_writeRepositoryMock.Object, _readRepositoryMock.Object, _unitOfWorkMock.Object, _mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidAddProductCommand_ShouldInvokeWriteRepositoryAddProductAsync()
    {
        Category? category = new Fixture().Create<Category>();
        _readRepositoryMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Category?>(category));
        Product product = new Fixture().Create<Product>();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>())).Returns(product);

        Result result = await _addProductCommandHandler.Handle(new Fixture().Create<AddProductCommand>(), It.IsAny<CancellationToken>());

        _writeRepositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
        Assert.True(result.Success);
        Assert.Equal(product.Id, result.Data);
    }

    [Fact]
    public async Task Handle_ProductCategoryNotFound_ShouldReturnWithFailed()
    {
        Result result = await _addProductCommandHandler.Handle(new Fixture().Create<AddProductCommand>(), It.IsAny<CancellationToken>());

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
