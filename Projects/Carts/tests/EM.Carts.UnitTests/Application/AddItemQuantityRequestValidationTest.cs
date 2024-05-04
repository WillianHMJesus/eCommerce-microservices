using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.AddItemQuantity.Validations;
using EM.Carts.UnitTests.Fixtures.Application;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class AddItemQuantityRequestValidationTest
{
    private readonly Mock<IAddItemQuantityUseCase> _addItemQuantityUseCaseMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly AddItemQuantityRequestValidation _addItemQuantityRequestValidation;
    private readonly AddItemQuantityRequest _addItemQuantityRequest;

    public AddItemQuantityRequestValidationTest()
    {
        _addItemQuantityUseCaseMock = new();
        _presenterMock = new();
        _addItemQuantityRequestValidation = new(_addItemQuantityUseCaseMock.Object);
        _addItemQuantityRequestValidation.SetPresenter(_presenterMock.Object);

        AddItemQuantityRequestFixture addItemQuantityRequestFixture = new();
        _addItemQuantityRequest = addItemQuantityRequestFixture.GenerateValidAddItemQuantityRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ValidAddItemQuantityRequest_MustInvokeExecuteAsync()
    {
        await _addItemQuantityRequestValidation.ExecuteAsync(_addItemQuantityRequest);

        _addItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<AddItemQuantityRequest>()), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemQuantityRequestWithoutUserId_MustReturnBadRequest()
    {
        _addItemQuantityRequest.UserId = Guid.Empty;

        await _addItemQuantityRequestValidation.ExecuteAsync(_addItemQuantityRequest);

        _addItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<AddItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemQuantityRequestWithoutProductId_MustReturnBadRequest()
    {
        _addItemQuantityRequest.ProductId = Guid.Empty;

        await _addItemQuantityRequestValidation.ExecuteAsync(_addItemQuantityRequest);

        _addItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<AddItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemQuantityRequestWithZeroQuantity_MustReturnBadRequest()
    {
        _addItemQuantityRequest.Quantity = 0;

        await _addItemQuantityRequestValidation.ExecuteAsync(_addItemQuantityRequest);

        _addItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<AddItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
