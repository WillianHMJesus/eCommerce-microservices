namespace EM.Catalog.Application.Interfaces;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}
