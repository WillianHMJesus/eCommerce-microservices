using EM.Carts.Domain.Entities;

namespace EM.Carts.Application.DTOs;

public sealed record CartDTO
{
    public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();

    public static explicit operator CartDTO(Cart cart)
    {
        return new CartDTO
        {
            Items = cart.Items.Select(x => (ItemDTO)x).ToList()
        };
    }
}
