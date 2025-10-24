using WH.SharedKernel;

namespace EM.Catalog.Application.Products.Events.ProductDeleted;

public sealed class ProductDeletedEvent
    : DomainEvent
{
    public Guid Id { get; set; }
}
