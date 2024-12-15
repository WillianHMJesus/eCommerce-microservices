namespace EM.Checkout.Application.Models;

public sealed record ItemDTO(Guid ProductId, string ProductName, string ProductImage, decimal Value, int Quantity)
{ }
