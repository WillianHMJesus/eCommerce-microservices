using AutoMapper;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public sealed class GetCartByUserIdUseCase : IUseCase<GetCartByUserIdRequest>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;
    private IPresenter _presenter = default!;

    public GetCartByUserIdUseCase(
        ICartRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(GetCartByUserIdRequest request, CancellationToken cancellationToken)
    {
        Cart? cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken);

        _presenter.Success(_mapper.Map<CartDTO>(cart));
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
