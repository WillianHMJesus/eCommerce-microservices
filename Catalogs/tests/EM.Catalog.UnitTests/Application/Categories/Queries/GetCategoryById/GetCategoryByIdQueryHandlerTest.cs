using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandlerTest
{
    [Fact]
    public async void Handle_ValidGetCategoryByIdQuery_ShouldInvokeReadRepositoryGetCategoryByIdAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        GetCategoryByIdQueryHandler getCategoryByIdQueryHandler = new(readRepositoryMock.Object, mapperMock.Object);

        CategoryDTO? categoryDTO = await getCategoryByIdQueryHandler.Handle(new Fixture().Create<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
