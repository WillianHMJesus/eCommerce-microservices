using EM.Carts.Domain.Entities;

namespace EM.Carts.Application.DTOs;

public sealed record ItemDTO(Guid ProductId, string ProductName, string ProductImage, decimal Value, int Quantity)
{
    public static explicit operator ItemDTO(Item item)
    {
        return new ItemDTO(
            item.ProductId,
            item.ProductName,
            item.ProductImage,
            item.Value,
            item.Quantity);
    }
}
