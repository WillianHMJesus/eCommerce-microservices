using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public class AddCategoryHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IProductRepository> mockProductRepository = new();
        AddCategoryHandler addCategoryHandler = new(mockProductRepository.Object);

        Result result = await addCategoryHandler.Handle(new AddCategoryCommand(10, "Informática", "Categoria de informática"), CancellationToken.None);

        mockProductRepository.Verify(x => x.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
        Assert.True(result.Success);
    }
}
