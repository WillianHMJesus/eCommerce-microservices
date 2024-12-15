using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EM.Checkout.Infraestructure.Persistense.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly CheckoutContext _context;

    public OrderRepository(CheckoutContext checkout)
        => _context = checkout;

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
