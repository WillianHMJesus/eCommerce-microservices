using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed record AddProductCommand(string Name, string Description, decimal Value, short Quantity, string Image, Guid CategoryId)
    : ICommand
{ }
