using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdQueryHandlerTest
{
    [Fact]
    public async void Handle_ValidGetProductsByCategoryIdQuery_ShouldInvokeReadRepositoryGetProductsByCategoryIdAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        GetProductsByCategoryIdQueryHandler getProductsByCategoryIdQueryHandler = new(readRepositoryMock.Object, mapperMock.Object);

        IEnumerable<ProductDTO> productDTOs = await getProductsByCategoryIdQueryHandler.Handle(new Fixture().Create<GetProductsByCategoryIdQuery>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.GetProductsByCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
