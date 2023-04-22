using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public sealed class UpdateCategoryHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IProductRepository> mockProductRepository = new();
        UpdateCategoryHandler updateCategoryHandler = new(mockProductRepository.Object);

        Result result = await updateCategoryHandler.Handle(new UpdateCategoryCommand(Guid.NewGuid(), 10, "Informática", "Categoria de informática"), 
            CancellationToken.None);

        mockProductRepository.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }
}
