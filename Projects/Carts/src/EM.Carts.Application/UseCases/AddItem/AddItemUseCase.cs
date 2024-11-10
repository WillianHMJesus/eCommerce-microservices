using AutoMapper;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.AddItem;

public sealed class AddItemUseCase : IUseCase<AddItemRequest>
{
    private readonly ICartRepository _repository;
    private readonly ICatalogExternalService _catalogExternalService;
    private readonly IMapper _mapper;
    private IPresenter _presenter = default!;

    public AddItemUseCase(
        ICartRepository repository,
        ICatalogExternalService catalogExternalService,
        IMapper mapper)
    { 
        _repository = repository;
        _catalogExternalService = catalogExternalService;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(AddItemRequest request, CancellationToken cancellationToken)
    {
        Cart cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken)
            ?? await CreateCart(request.UserId, cancellationToken);

        ProductDTO product = await _catalogExternalService.GetProductsByIdAsync(request.ProductId, cancellationToken)
            ?? throw new ArgumentNullException();

        Item? existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);

        if (existingItem is null)
        {
            Item item = _mapper.Map<Item>((request, product));
            cart.AddItem(item);
        }
        else
        {
            existingItem.AddQuantity(request.Quantity);
        }

        await _repository.UpdateCartAsync(cart, cancellationToken);
        _presenter.Success();
    }

    private async Task<Cart> CreateCart(Guid userId, CancellationToken cancellationToken)
    {
        Cart cart = new Cart(userId);
        await _repository.AddCartAsync(cart, cancellationToken);

        return cart;
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
