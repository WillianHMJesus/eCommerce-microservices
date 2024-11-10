using EM.Carts.Application.Interfaces.UseCases;
using System.Text.Json.Serialization;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public sealed record GetCartByUserIdRequest : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
