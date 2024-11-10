namespace EM.Carts.Application.DTOs;

public sealed record ItemDTO(Guid ProductId, string ProductName, string ProductImage, decimal Value, int Quantity)
{
    public decimal TotalValue => Value * Quantity;
}
