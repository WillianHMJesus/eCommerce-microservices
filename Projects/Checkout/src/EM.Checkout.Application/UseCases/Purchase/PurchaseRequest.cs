using System.Text.Json.Serialization;

namespace EM.Checkout.Application.UseCases.Purchase;

public class PurchaseRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string CardHolderCpf { get; set; } = ""!;
    public string CardHolderName { get; set; } = ""!;
    public string CardNumber { get; set; } = ""!;
    public string CardExpirationDate { get; set; } = ""!;
    public string CardSecurityCode { get; set; } = ""!;
}
