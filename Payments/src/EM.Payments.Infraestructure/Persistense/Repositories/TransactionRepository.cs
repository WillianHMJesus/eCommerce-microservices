using EM.Payments.Domain.Entities;
using EM.Payments.Domain.Interfaces;

namespace EM.Payments.Infraestructure.Persistense.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly PaymentContext _context;

    public TransactionRepository(PaymentContext context)
        => _context = context;

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}
