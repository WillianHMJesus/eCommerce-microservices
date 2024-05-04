using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.DeleteItem.Validations;
using EM.Carts.UnitTests.Fixtures.Application;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class DeleteItemRequestValidationTest
{
    private readonly Mock<IDeleteItemUseCase> _deleteItemUseCaseMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly DeleteItemRequestValidation _deleteItemRequestValidation;
    private readonly DeleteItemRequest _deleteItemRequest;

    public DeleteItemRequestValidationTest()
    {
        _deleteItemUseCaseMock = new();
        _presenterMock = new();
        _deleteItemRequestValidation = new(_deleteItemUseCaseMock.Object);
        _deleteItemRequestValidation.SetPresenter(_presenterMock.Object);

        DeleteItemRequestFixture deleteItemRequestFixture = new();
        _deleteItemRequest = deleteItemRequestFixture.GenerateValidDeleteItemRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ValidDeleteItemRequest_MustInvokeExecuteAsync()
    {
        await _deleteItemRequestValidation.ExecuteAsync(_deleteItemRequest);

        _deleteItemUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<DeleteItemRequest>()), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeleteItemRequestWithoutUserId_MustReturnBadRequest()
    {
        _deleteItemRequest.UserId = Guid.Empty;

        await _deleteItemRequestValidation.ExecuteAsync(_deleteItemRequest);

        _deleteItemUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<DeleteItemRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeleteItemRequestWithoutProductId_MustReturnBadRequest()
    {
        _deleteItemRequest.ProductId = Guid.Empty;

        await _deleteItemRequestValidation.ExecuteAsync(_deleteItemRequest);

        _deleteItemUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<DeleteItemRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
