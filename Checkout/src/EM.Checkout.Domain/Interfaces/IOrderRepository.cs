using EM.Checkout.Domain.Entities;

namespace EM.Checkout.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken);
}
