using EM.Catalog.Application.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.CategoryUpdated;

public sealed record CategoryUpdatedEvent(Guid Id, short Code, string Name, string Description) : IEvent
{
    public static explicit operator CategoryUpdatedEvent(Category category)
    {
        return new CategoryUpdatedEvent(
            category.Id,
            category.Code,
            category.Name,
            category.Description);
    }

    public static explicit operator CategoryDTO(CategoryUpdatedEvent categoryUpdatedEvent)
    {
        return new CategoryDTO(
            categoryUpdatedEvent.Id,
            categoryUpdatedEvent.Code,
            categoryUpdatedEvent.Name,
            categoryUpdatedEvent.Description);
    }
}
