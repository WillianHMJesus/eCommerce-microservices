namespace EM.Carts.Application.DTOs;

public sealed record CartDTO
{
    public decimal TotalValue => Items.Sum(x => x.TotalValue);
    public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
}
