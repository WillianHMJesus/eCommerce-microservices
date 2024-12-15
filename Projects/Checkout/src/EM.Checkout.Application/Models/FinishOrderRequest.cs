namespace EM.Checkout.Application.Models;

public sealed record FinishOrderRequest
{
    public string CardHolderCpf { get; set; } = ""!;
    public string CardHolderName { get; set; } = ""!;
    public string CardNumber { get; set; } = ""!;
    public string CardExpirationDate { get; set; } = ""!;
    public string CardSecurityCode { get; set; } = ""!;
}
