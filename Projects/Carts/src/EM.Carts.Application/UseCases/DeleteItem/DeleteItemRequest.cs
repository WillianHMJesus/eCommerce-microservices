using EM.Carts.Application.Interfaces.UseCases;
using System.Text.Json.Serialization;

namespace EM.Carts.Application.UseCases.DeleteItem;

public sealed record DeleteItemRequest : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}
