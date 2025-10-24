using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand
{ }
