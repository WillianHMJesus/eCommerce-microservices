using AutoMapper;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public sealed class AddCategoryHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IWriteRepository> mockWriteRepository = new();
        Mock<IMapper> mockMapper = new();
        AddCategoryHandler addCategoryHandler = new(mockWriteRepository.Object, mockMapper.Object);
        mockMapper.Setup(x => x.Map<Category>(It.IsAny<AddCategoryCommand>()))
            .Returns(new CategoryFixture().GenerateCategory());

        Result result = await addCategoryHandler.Handle(new AddCategoryCommand(10, "Informática", "Categoria de informática"), 
            CancellationToken.None);

        mockWriteRepository.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }
}
