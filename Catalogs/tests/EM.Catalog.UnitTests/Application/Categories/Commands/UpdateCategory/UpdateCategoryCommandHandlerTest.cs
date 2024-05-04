using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Shared.Core;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandlerTest
{
    private readonly Mock<IWriteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UpdateCategoryCommandHandler _updateCategoryCommandHandler;
    private readonly UpdateCategoryCommand _updateCategoryCommand;

    public UpdateCategoryCommandHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IWriteRepository>>();
        _unitOfWorkMock = fixture.Freeze<Mock<IUnitOfWork>>();
        _mediatorMock = fixture.Freeze<Mock<IMediator>>();
        Category category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<UpdateCategoryCommand>()))
            .Returns(category);

        _updateCategoryCommandHandler = fixture.Create<UpdateCategoryCommandHandler>();
        _updateCategoryCommand = new Fixture().Create<UpdateCategoryCommand>();
    }

    [Fact]
    public async Task Handle_ValidCommit_MustReturnWithSuccess()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        Result result = await _updateCategoryCommandHandler.Handle(_updateCategoryCommand, CancellationToken.None);

        _repositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        _mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidCommit_ShouldThrowDomainException()
    {
        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));

        DomainException domainException = await Assert.ThrowsAsync<DomainException>(
            async () => await _updateCategoryCommandHandler.Handle(_updateCategoryCommand, CancellationToken.None));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryAnErrorOccorred);
    }
}
