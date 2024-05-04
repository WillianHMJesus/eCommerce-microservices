using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;

namespace EM.Checkout.Infraestructure.Persistense.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly CheckoutContext _context;

    public OrderRepository(CheckoutContext checkout)
        => _context = checkout;

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}
