using EM.Carts.Application.Interfaces.UseCases;
using System.Text.Json.Serialization;

namespace EM.Carts.Application.UseCases.AddItem;

public sealed record AddItemRequest : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
