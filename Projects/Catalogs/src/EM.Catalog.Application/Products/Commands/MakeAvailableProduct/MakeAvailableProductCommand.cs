using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Products.Commands.MakeAvailableProduct;

public sealed record MakeAvailableProductCommand(Guid Id)
    : ICommand
{ }
