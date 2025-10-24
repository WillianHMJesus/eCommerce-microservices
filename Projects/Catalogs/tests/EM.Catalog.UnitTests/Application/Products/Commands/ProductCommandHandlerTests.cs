using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Commands;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using EM.Catalog.Application.Products.Commands.InactivateProduct;
using EM.Catalog.Application.Products.Commands.ReactivateProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using Moq;
using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands;

public sealed class ProductCommandHandlerTests
{
    [Theory, AutoProductData]
    [Trait("Test", "AddProduct:AddNewProduct")]
    public async Task AddProduct_AddNewProduct_ShouldAddProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        AddProductCommand command,
        Product product)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(product.Id);
    }

    [Theory, AutoProductData]
    [Trait("Test", "AddProduct:ReturnFalseCommit")]
    public async Task AddProduct_ReturnFalseCommit_ShouldNotAddProductAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        AddProductCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Product.ErrorSavingProduct);
    }

    [Theory, AutoProductData]
    [Trait("Test", "DeleteProduct:DeleteExitingProduct")]
    public async Task DeleteProduct_DeleteExitingProduct_ShouldDeleteProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        DeleteProductCommand command)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "DeleteProduct:ProductNotFound")]
    public async Task DeleteProduct_ProductNotFound_ShouldNotDeleteProductAndReturnDomainException(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        DeleteProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Product);

        //Act
        Exception domainException = await Record.ExceptionAsync(() => sut.Handle(command, CancellationToken.None));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ProductNotFound);
        repositoryMock.Verify(x => x.Delete(It.IsAny<Product>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory, AutoProductData]
    [Trait("Test", "DeleteProduct:ReturnFalseCommit")]
    public async Task DeleteProduct_ReturnFalseCommit_ShouldNotDeleteProductAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        DeleteProductCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Product.ErrorSavingProduct);
    }

    [Theory, AutoProductData]
    [Trait("Test", "ReactivateProduct:ReactivateInactiveProduct")]
    public async Task ReactivateProduct_ReactivateInactiveProduct_ShouldReactivateProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        ReactivateProductCommand command,
        Product product)
    {
        //Assert
        product.Inactivate();
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "ReactivateProduct:ReactivateActiveProduct")]
    public async Task ReactivateProduct_ReactivateActiveProduct_ShouldReactivateProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        ReactivateProductCommand command,
        Product product)
    {
        //Assert
        product.Reactivate();
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "ReactivateProduct:ProductNotFound")]
    public async Task ReactivateProduct_ProductNotFound_ShouldNotReactivateProductAndReturnDomainException(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        ReactivateProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Product);

        //Act
        Exception domainException = await Record.ExceptionAsync(() => sut.Handle(command, CancellationToken.None));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ProductNotFound);
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory, AutoProductData]
    [Trait("Test", "ReactivateProduct:ReturnFalseCommit")]
    public async Task ReactivateProduct_ReturnFalseCommit_ShouldNotReactivateProductAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        ReactivateProductCommand command,
        Product product)
    {
        //Arrange
        product.Inactivate();
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Product.ErrorSavingProduct);
    }

    [Theory, AutoProductData]
    [Trait("Test", "InactivateProduct:InactivateActiveProduct")]
    public async Task InactivateProduct_InactivateActiveProduct_ShouldActivateProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        InactivateProductCommand command,
        Product product)
    {
        //Assert
        product.Reactivate();
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "InactivateProduct:InactivateInactiveProduct")]
    public async Task InactivateProduct_InactivateInactiveProduct_ShouldActivateProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        InactivateProductCommand command,
        Product product)
    {
        //Assert
        product.Inactivate();
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "InactivateProduct:ProductNotFound")]
    public async Task InactivateProduct_ProductNotFound_ShouldNotInactivateProductAndReturnDomainException(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        InactivateProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Product);

        //Act
        Exception domainException = await Record.ExceptionAsync(() => sut.Handle(command, CancellationToken.None));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ProductNotFound);
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory, AutoProductData]
    [Trait("Test", "InactivateProduct:ReturnFalseCommit")]
    public async Task InactivateProduct_ReturnFalseCommit_ShouldNotReactivateProductAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        InactivateProductCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAvailability(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Product.ErrorSavingProduct);
    }

    [Theory, AutoProductData]
    [Trait("Test", "UpdateProduct:UpdateExistingProduct")]
    public async Task UpdateProduct_UpdateExistingProduct_ShouldUpdateProductAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        UpdateProductCommand command)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "UpdateProduct:ReturnFalseCommit")]
    public async Task UpdateProduct_ReturnFalseCommit_ShouldNotUpdateProductAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        ProductCommandHandler sut,
        UpdateProductCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Product.ErrorSavingProduct);
    }
}
