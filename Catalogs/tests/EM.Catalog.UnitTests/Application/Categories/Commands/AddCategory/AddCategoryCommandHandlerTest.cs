using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Shared.Core;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AddCategoryCommandHandler _addCategoryCommandHandler;
    private readonly AddCategoryCommand _addCategoryCommand;
    private readonly Category _category;

    public AddCategoryCommandHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IWriteRepository>>();
        _unitOfWorkMock = fixture.Freeze<Mock<IUnitOfWork>>();
        _mediatorMock = fixture.Freeze<Mock<IMediator>>();
        _category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<AddCategoryCommand>()))
            .Returns(_category);

        _addCategoryCommandHandler = fixture.Create<AddCategoryCommandHandler>();
        _addCategoryCommand = new Fixture().Create<AddCategoryCommand>();
    }

    [Fact]
    public async Task Handle_ValidCommit_ShouldReturnWithSuccess()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        Result result = await _addCategoryCommandHandler.Handle(_addCategoryCommand, CancellationToken.None);

        _repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        _mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(_category.Id);
    }

    [Fact]
    public async Task Handle_InvalidCommit_ShouldThrowDomainException()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));

        DomainException domainException = await Assert.ThrowsAsync<DomainException>(
            async () => await _addCategoryCommandHandler.Handle(_addCategoryCommand, CancellationToken.None));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryAnErrorOccorred);
    }
}
