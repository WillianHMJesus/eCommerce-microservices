using WH.SharedKernel;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedEvent : DomainEvent
{
    public Guid Id { get; set; }
    public short Code { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}
