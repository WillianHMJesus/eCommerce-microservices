namespace EM.Carts.Application.UseCases.AddItem;

public class AddItemRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = ""!;
    public string ProductImage { get; set; } = ""!;
    public decimal Value { get; set; }
    public int Quantity { get; set; }
}
