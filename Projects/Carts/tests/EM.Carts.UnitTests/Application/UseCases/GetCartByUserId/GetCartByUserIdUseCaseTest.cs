using AutoFixture.Xunit2;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.GetCartByUserId;

public sealed class GetCartByUserIdUseCaseTest
{
    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCart_ShouldInvokeGetCartByUserIdAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        GetCartByUserIdUseCase sut,
        GetCartByUserIdRequest request)
    {
        sut.SetPresenter(presenterMock.Object);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(It.IsAny<object>()), Times.Once);
    }
}
