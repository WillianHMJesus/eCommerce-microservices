using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

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
