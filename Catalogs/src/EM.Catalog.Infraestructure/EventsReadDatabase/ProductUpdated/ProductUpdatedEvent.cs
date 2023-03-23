using EM.Catalog.Application.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductUpdated;

public sealed record ProductUpdatedEvent(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, Category Category)
    : IEvent
{
    public static explicit operator ProductUpdatedEvent(Product product)
    {
        return new ProductUpdatedEvent(
            product.Id,
            product.Name,
            product.Description,
            product.Value,
            product.Quantity,
            product.Image,
            product.Category!);
    }

    public static explicit operator ProductDTO(ProductUpdatedEvent productAddedEvent)
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
