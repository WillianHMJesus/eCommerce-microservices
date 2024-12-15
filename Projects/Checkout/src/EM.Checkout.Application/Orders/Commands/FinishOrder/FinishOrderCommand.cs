using EM.Checkout.Application.Interfaces.Commands;

namespace EM.Checkout.Application.Orders.Commands.FinishOrder;

public sealed record FinishOrderCommand(Guid UserId, string CardHolderCpf, string CardHolderName, string CardNumber, string CardExpirationDate, string CardSecurityCode)
    : ICommand
{ }
