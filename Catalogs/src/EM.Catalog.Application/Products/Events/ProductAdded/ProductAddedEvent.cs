using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Products.Events.ProductAdded;

public sealed record ProductAddedEvent(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, Category Category)
    : IEvent
{
    public static explicit operator ProductAddedEvent(Product product)
    {
        return new ProductAddedEvent(
            product.Id,
            product.Name,
            product.Description,
            product.Value,
            product.Quantity,
            product.Image,
            product.Category!);
    }

    public static explicit operator ProductDTO(ProductAddedEvent productAddedEvent)
    {
        return new ProductDTO(
            productAddedEvent.Id,
            productAddedEvent.Name,
            productAddedEvent.Description,
            productAddedEvent.Value,
            productAddedEvent.Quantity,
            productAddedEvent.Image)
        {
            Category = (CategoryDTO)productAddedEvent.Category
        };
    }
}
