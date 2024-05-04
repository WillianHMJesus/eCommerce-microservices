using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.SubtractItemQuantity;
using EM.Carts.Application.UseCases.SubtractItemQuantity.Validations;
using EM.Carts.UnitTests.Fixtures.Application;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class SubtractItemQuantityRequestValidationTest
{
    private readonly Mock<ISubtractItemQuantityUseCase> _subtractItemQuantityUseCaseMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly SubtractItemQuantityRequestValidation _subtractItemQuantityRequestValidation;
    private readonly SubtractItemQuantityRequest _subtractItemQuantityRequest;

    public SubtractItemQuantityRequestValidationTest()
    {
        _subtractItemQuantityUseCaseMock = new();
        _presenterMock = new();
        _subtractItemQuantityRequestValidation = new(_subtractItemQuantityUseCaseMock.Object);
        _subtractItemQuantityRequestValidation.SetPresenter(_presenterMock.Object);

        SubtractItemQuantityRequestFixture deleteItemRequestFixture = new();
        _subtractItemQuantityRequest = deleteItemRequestFixture.GenerateValidSubtractItemQuantityRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ValidSubtractItemQuantityRequest_MustInvokeExecuteAsync()
    {
        await _subtractItemQuantityRequestValidation.ExecuteAsync(_subtractItemQuantityRequest);

        _subtractItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<SubtractItemQuantityRequest>()), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_SubtractItemQuantityRequestWithoutUserId_MustReturnBadRequest()
    {
        _subtractItemQuantityRequest.UserId = Guid.Empty;

        await _subtractItemQuantityRequestValidation.ExecuteAsync(_subtractItemQuantityRequest);

        _subtractItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<SubtractItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_SubtractItemQuantityRequestWithoutProductId_MustReturnBadRequest()
    {
        _subtractItemQuantityRequest.ProductId = Guid.Empty;

        await _subtractItemQuantityRequestValidation.ExecuteAsync(_subtractItemQuantityRequest);

        _subtractItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<SubtractItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_SubtractItemQuantityRequestWithZeroQuantity_MustReturnBadRequest()
    {
        _subtractItemQuantityRequest.Quantity = 0;

        await _subtractItemQuantityRequestValidation.ExecuteAsync(_subtractItemQuantityRequest);

        _subtractItemQuantityUseCaseMock.Verify(x => x.ExecuteAsync(It.IsAny<SubtractItemQuantityRequest>()), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
