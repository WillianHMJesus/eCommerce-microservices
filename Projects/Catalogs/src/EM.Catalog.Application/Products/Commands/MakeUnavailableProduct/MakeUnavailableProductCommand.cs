using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Products.Commands.MakeUnavailableProduct;

public sealed record MakeUnavailableProductCommand(Guid Id)
    : ICommand
{ }
