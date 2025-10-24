using WH.SharedKernel;

namespace EM.Catalog.Application.Categories.Events.CategoryDeleted;

public sealed class CategoryDeletedEvent : DomainEvent
{
    public Guid Id { get; set; }
}
