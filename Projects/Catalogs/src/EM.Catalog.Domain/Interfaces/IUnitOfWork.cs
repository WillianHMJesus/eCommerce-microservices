﻿namespace EM.Catalog.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}
