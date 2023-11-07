namespace EM.Shared.Core.MessageBrokers.Contracts;

public sealed record OrderCreated
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public decimal Value { get; set; }
    public string CardHolderCpf { get; set; } = ""!;
    public string CardHolderName { get; set; } = ""!;
    public string CardNumber { get; set; } = ""!;
    public string CardExpirationDate { get; set; } = ""!;
    public string CardSecurityCode { get; set; } = ""!;
}
