using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IWriteRepository> mockWriteRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        Mock<IMediator> mockMediator = new();
        Mock<IMapper> mockMapper = new();
        UpdateCategoryCommandHandler updateCategoryHandler = new(mockWriteRepository.Object, mockUnitOfWork.Object, mockMediator.Object, mockMapper.Object);

        Result result = await updateCategoryHandler.Handle(new Fixture().Create<UpdateCategoryCommand>(), CancellationToken.None);

        mockWriteRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        Assert.True(result.Success);
    }
}
