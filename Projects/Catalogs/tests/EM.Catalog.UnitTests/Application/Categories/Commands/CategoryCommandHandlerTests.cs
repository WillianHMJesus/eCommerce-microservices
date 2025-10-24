using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using Moq;
using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands;

public sealed class CategoryCommandHandlerTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "AddCategory:AddNewCategory")]
    public async Task AddCategory_AddNewCategory_ShouldAddCategoryAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        AddCategoryCommand command,
        Category category)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(category.Id);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "AddCategory:ReturnFalseCommit")]
    public async Task AddCategory_ReturnFalseCommit_ShouldNotAddCategoryAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        AddCategoryCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Category.ErrorSavingCategory);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "DeleteCategory:DeleteExistingCategory")]
    public async Task DeleteCategory_DeleteExistingCategory_ShouldDeleteCategoryAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        DeleteCategoryCommand command)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "DeleteCategory:CategoryNotFound")]
    public async Task DeleteCategory_CategoryNotFound_ShouldNotDeleteCategoryAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        DeleteCategoryCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Category.CategoryNotFound);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "DeleteCategory:ReturnFalseCommit")]
    public async Task DeleteCategory_ReturnFalseCommit_ShouldNotDeleteCategoryAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        DeleteCategoryCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Category.ErrorSavingCategory);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "UpdateCategory:UpdateExistingCategory")]
    public async Task UpdateCategory_UpdateExistingCategory_ShouldUpdateCategoryAndReturnSuccess(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        UpdateCategoryCommand command)
    {
        //Arrange & Act
        Result result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "UpdateCategory:ReturnFalseCommit")]
    public async Task UpdateCategory_ReturnFalseCommit_ShouldNotUpdateCategoryAndReturnFailure(
        [Frozen] Mock<IProductRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        CategoryCommandHandler sut,
        UpdateCategoryCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        Result result = await sut.Handle(command, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish<IEvent>(It.IsAny<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Category.ErrorSavingCategory);
    }
}
