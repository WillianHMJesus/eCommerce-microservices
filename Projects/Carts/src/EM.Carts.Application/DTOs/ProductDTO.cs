namespace EM.Carts.Application.DTOs;

public sealed record ProductDTO(Guid Id, string Name, decimal Value, short Quantity, string Image, bool Available)
{ }
