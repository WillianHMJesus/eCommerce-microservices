using EM.Checkout.Domain.Entities;

namespace EM.Checkout.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Order order, CancellationToken cancellationToken);
}
