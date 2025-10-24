using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Commands.ReactivateProduct;

public sealed record ReactivateProductCommand(Guid Id)
    : ICommand
{ }
