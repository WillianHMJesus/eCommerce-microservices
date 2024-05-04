using EM.Payments.Domain.Entities;

namespace EM.Payments.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
}
