using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Commands.InactivateProduct;

public sealed record InactivateProductCommand(Guid Id)
    : ICommand
{ }
