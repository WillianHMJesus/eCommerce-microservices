using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTest
{
    [Fact]
    public async void Handle_ValidGetProductByIdQuery_ShouldInvokeReadRepositoryGetProductByIdAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        GetProductByIdQueryHandler getProductByIdQueryHandler = new(readRepositoryMock.Object, mapperMock.Object);

        ProductDTO? productDTO = await getProductByIdQueryHandler.Handle(new Fixture().Create<GetProductByIdQuery>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
