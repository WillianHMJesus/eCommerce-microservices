using System.Text.Json.Serialization;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity;

public sealed record SubtractItemQuantityRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
