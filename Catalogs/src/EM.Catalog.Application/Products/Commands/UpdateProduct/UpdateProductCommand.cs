using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image)
    : ICommand
{ }
