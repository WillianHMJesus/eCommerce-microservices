using EM.Carts.Application.Interfaces.UseCases;
using System.Text.Json.Serialization;

namespace EM.Carts.Application.UseCases.DeleteAllItems;

public sealed record DeleteAllItemsRequest : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
