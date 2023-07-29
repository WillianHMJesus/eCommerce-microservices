using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItem.Validations;
using EM.Carts.UnitTests.Fixtures.Application;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class AddItemRequestValidationTest
{
    private readonly Mock<IAddItemUseCase> _mockAddItemUseCase;
    private readonly Mock<IPresenter> _mockPresenter;
    private readonly AddItemRequestValidation _addItemRequestValidation;
    private readonly AddItemRequest _addItemRequest;

    public AddItemRequestValidationTest()
    {
        _mockAddItemUseCase = new();
        _mockPresenter = new();
        _addItemRequestValidation = new(_mockAddItemUseCase.Object);
        _addItemRequestValidation.SetPresenter(_mockPresenter.Object);

        AddItemRequestFixture addItemRequestFixture = new();
        _addItemRequest = addItemRequestFixture.GenerateValidAddItemRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ValidAddItemRequest_MustInvokeExecuteAsync()
    {
        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Once);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithoutUserId_MustInvokeRetornBadRequest()
    {
        _addItemRequest.UserId = Guid.Empty;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithoutProductId_MustInvokeRetornBadRequest()
    {
        _addItemRequest.ProductId = Guid.Empty;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithoutProductName_MustInvokeRetornBadRequest()
    {
        _addItemRequest.ProductName = string.Empty;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithoutProductImage_MustInvokeRetornBadRequest()
    {
        _addItemRequest.ProductImage = string.Empty;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithZeroValue_MustInvokeRetornBadRequest()
    {
        _addItemRequest.Value = 0;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_AddItemRequestWithZeroQuantity_MustInvokeRetornBadRequest()
    {
        _addItemRequest.Quantity = 0;

        await _addItemRequestValidation.ExecuteAsync(_addItemRequest);

        _mockAddItemUseCase.Verify(x => x.ExecuteAsync(It.IsAny<AddItemRequest>()), Times.Never);
        _mockPresenter.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
