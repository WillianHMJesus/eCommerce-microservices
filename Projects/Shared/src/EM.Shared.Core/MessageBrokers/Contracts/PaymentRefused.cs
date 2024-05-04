namespace EM.Shared.Core.MessageBrokers.Contracts;

public sealed record PaymentRefused
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
