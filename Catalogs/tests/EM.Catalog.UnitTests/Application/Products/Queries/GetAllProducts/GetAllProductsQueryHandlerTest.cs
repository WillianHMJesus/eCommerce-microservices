using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandlerTest
{
    [Fact]
    public async void Handle_ValidGetAllProductsQuery_ShouldInvokeReadRepositoryGetAllProductsAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        GetAllProductsQueryHandler getAllProductsQueryHandler = new(readRepositoryMock.Object, mapperMock.Object);

        IEnumerable<ProductDTO> productDTOs = await getAllProductsQueryHandler.Handle(new Fixture().Create<GetAllProductsQuery>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.GetAllProductsAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
