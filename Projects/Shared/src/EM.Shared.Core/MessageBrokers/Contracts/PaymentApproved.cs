namespace EM.Shared.Core.MessageBrokers.Contracts;

public sealed record PaymentApproved
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
