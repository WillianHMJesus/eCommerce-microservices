using WH.SharedKernel;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedEvent : DomainEvent
{
    public Guid Id { get; set; }
    public short Code { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}
