using EM.Catalog.Application.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductAdded;

public sealed record ProductAddedEvent(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, bool Available, Category Category)
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
            product.Available,
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
            productAddedEvent.Image,
            productAddedEvent.Available)
        {
            Category = (CategoryDTO)productAddedEvent.Category
        };
    }
}
