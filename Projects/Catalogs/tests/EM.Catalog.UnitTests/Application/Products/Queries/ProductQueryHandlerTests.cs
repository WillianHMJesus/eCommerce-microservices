using AutoFixture.Xunit2;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Queries;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Products.Queries.SearchProducts;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries;

public sealed class ProductQueryHandlerTests
{
    [Theory, AutoProductData]
    [Trait("Test", "GetAllProducts:ValidGetAllProductsQuery")]
    public async Task GetAllProducts_ValidGetAllProductsQuery_ShouldInvokeReadRepositoryGetAllProductsAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductQueryHandler sut,
        GetAllProductsQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.GetAllAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoProductData]
    [Trait("Test", "GetProductById:ValidGetProductByIdQuery")]
    public async Task GetProductById_ValidGetProductByIdQuery_ShouldInvokeReadRepositoryGetProductByIdAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductQueryHandler sut,
        GetProductByIdQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoProductData]
    [Trait("Test", "GetProductsByCategoryId:ValidGetProductsByCategoryIdQuery")]
    public async Task GetProductsByCategoryId_ValidGetProductsByCategoryIdQuery_ShouldInvokeReadRepositoryGetProductsByCategoryIdAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductQueryHandler sut,
        GetProductsByCategoryIdQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.GetByCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoProductData]
    [Trait("Test", "SearchProducts:ValidSearchProductsQuery")]
    public async Task SearchProducts_ValidSearchProductsQuery_ShouldInvokeReadRepositorySearchProductsAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductQueryHandler sut,
        SearchProductsQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
