using EM.Checkout.Domain.Entities;

namespace EM.Checkout.Application.DTOs;

public sealed record ItemDTO(Guid ProductId, string ProductName, string ProductImage, decimal Value, int Quantity)
{
    public static explicit operator Item(ItemDTO itemDTO)
    {
        return new Item(
            itemDTO.ProductId,
            itemDTO.ProductName,
            itemDTO.ProductImage,
            itemDTO.Quantity,
            itemDTO.Value);
    }
}
