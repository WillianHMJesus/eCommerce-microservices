using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, Guid CategoryId)
    : ICommand
{ }
