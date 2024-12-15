namespace EM.Checkout.Application.Models;

public sealed record CartDTO
{
    public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
}
