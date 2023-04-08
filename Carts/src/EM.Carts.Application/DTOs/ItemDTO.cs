using EM.Carts.Domain.Entities;

namespace EM.Carts.Application.DTOs;

public class ItemDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string ProductImage { get; set; } = default!;
    public decimal Value { get; set; }
    public int Quantity { get; set; }

    public static explicit operator ItemDTO(Item item)
    {
        return new ItemDTO
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Value = item.Value,
            Quantity = item.Quantity,
        };
    }
}
