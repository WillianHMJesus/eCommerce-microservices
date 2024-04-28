using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandlerTest
{
    [Fact]
    public async void Handle_ValidGetAllCategoriesQuery_ShouldInvokeReadRepositoryGetAllCategoriesAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        GetAllCategoriesQueryHandler getAllCategoriesQueryHandler = new(readRepositoryMock.Object, mapperMock.Object);

        IEnumerable<CategoryDTO> categoryDTOs = await getAllCategoriesQueryHandler.Handle(new Fixture().Create<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.GetAllCategoriesAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
