using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public sealed record CategoryAddedEvent(Guid Id, short Code, string Name, string Description) : IEvent
{
    public static explicit operator CategoryAddedEvent(Category category)
    {
        return new CategoryAddedEvent(
            category.Id, 
            category.Code, 
            category.Name, 
            category.Description);
    }

    public static explicit operator CategoryDTO(CategoryAddedEvent categoryAddedEvent)
    {
        return new CategoryDTO(
            categoryAddedEvent.Id,
            categoryAddedEvent.Code,
            categoryAddedEvent.Name,
            categoryAddedEvent.Description);
    }
}
