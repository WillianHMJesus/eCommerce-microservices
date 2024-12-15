using AutoMapper;
using EM.Checkout.Application.Interfaces.Commands;
using EM.Checkout.Application.Interfaces.ExternalServices;
using EM.Checkout.Application.Models;
using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;
using EM.Common.Core.Events;
using EM.Common.Core.MessageBrokers;
using EM.Common.Core.ResourceManagers;

namespace EM.Checkout.Application.Orders.Commands.FinishOrder;

public sealed class FinishOrderCommandHandler : ICommandHandler<FinishOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartExternalService _cartExternalService;
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly IResourceManager _resourceManager;
    private readonly IMapper _mapper;

    public FinishOrderCommandHandler(
        IOrderRepository orderRepository,
        ICartExternalService cartExternalService,
        IMessageBrokerService messageBrokerService,
        IResourceManager resourceManager,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _cartExternalService = cartExternalService;
        _messageBrokerService = messageBrokerService;
        _resourceManager = resourceManager;
        _mapper = mapper;
    }

    public async Task<Result> Handle(FinishOrderCommand request, CancellationToken cancellationToken)
    {
        CartDTO? cart = await _cartExternalService.GetItemsByUserId(request.UserId, cancellationToken);

        if (cart?.Items.Count == 0)
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.CartNotFound, cancellationToken);
        }

        Order order = new(request.UserId, GenerateOrderNumber());
        cart!.Items.ForEach(x => order.AddItem(_mapper.Map<Item>(x)));
        await _orderRepository.AddAsync(order, cancellationToken);

        OrderCreatedEvent _event = _mapper.Map<OrderCreatedEvent>((order, request));
        await _messageBrokerService.SendMessage(_event, cancellationToken);

        return Result.CreateResponseWithData();
    }

    private string GenerateOrderNumber()
    {
        return string.Format("ORD-{0}", DateTime.Now.ToString("yyyyMMdd.HHmmssfff"));
    }
}
