using EM.Checkout.Application.DTOs;
using EM.Checkout.Application.Interfaces;
using EM.Checkout.Application.MessageBrokers;
using EM.Checkout.Domain;
using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;
using EM.Shared.Core.MessageBrokers.Contracts;

namespace EM.Checkout.Application.UseCases.Purchase;

public class PurchaseUseCase : IPurchaseUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartExternalService _cartExternalService;
    private readonly IMessageBrokerService _messageBrokerService;
    private IPresenter _presenter = default!;

    public PurchaseUseCase(
        IOrderRepository orderRepository,
        ICartExternalService cartExternalService,
        IMessageBrokerService messageBrokerService)
    {
        _orderRepository = orderRepository;
        _cartExternalService = cartExternalService;
        _messageBrokerService = messageBrokerService;
    }

    public async Task ExecuteAsync(PurchaseRequest request, CancellationToken cancellationToken)
    {
        List<ItemDTO> itemsDTO = await _cartExternalService.GetItemsByUserId(request.UserId, cancellationToken);

        if (itemsDTO.Count == 0)
        {
            _presenter.BadRequest(new { ErrorMessage = ErrorMessage.CartNotFound });
            return;
        }

        Order order = new(request.UserId);
        itemsDTO.ForEach(x => order.AddItem((Item)x));

        await _orderRepository.AddOrderAsync(order, cancellationToken);

        await SendMessageOrderCreated(order, request, cancellationToken);

        _presenter.Accepted();
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;

    private async Task SendMessageOrderCreated(Order order, PurchaseRequest request, CancellationToken cancellationToken)
    {
        await _messageBrokerService.SendMessage(new OrderCreated
        {
            UserId = request.UserId,
            OrderId = order.Id,
            Value = order.Amount,
            CardHolderCpf = request.CardHolderCpf,
            CardHolderName = request.CardHolderName,
            CardNumber = request.CardNumber,
            CardExpirationDate = request.CardExpirationDate,
            CardSecurityCode = request.CardSecurityCode
        }, cancellationToken);
    }
}
