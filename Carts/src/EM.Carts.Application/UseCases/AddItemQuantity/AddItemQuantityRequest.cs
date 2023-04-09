namespace EM.Carts.Application.UseCases.AddItemQuantity;

public class AddItemQuantityRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
