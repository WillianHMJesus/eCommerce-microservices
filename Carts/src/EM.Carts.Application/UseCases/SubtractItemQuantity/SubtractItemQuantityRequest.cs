namespace EM.Carts.Application.UseCases.SubtractItemQuantity;

public class SubtractItemQuantityRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
