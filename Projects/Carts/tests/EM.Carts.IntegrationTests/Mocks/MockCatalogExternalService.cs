using AutoFixture;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;

namespace EM.Carts.IntegrationTests.Mocks;

public sealed class MockCatalogExternalService : ICatalogExternalService
{
    public Task<ProductDTO?> GetProductsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Fixture().Build<ProductDTO?>()
            .With(x => x.Quantity, short.MaxValue - 1)
            .With(x => x.Available, true)
            .Create());
    }
}
