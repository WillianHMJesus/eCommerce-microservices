using AutoMapper;
using EM.Checkout.Application.Models;
using EM.Checkout.Application.Orders.Commands.FinishOrder;

namespace EM.Checkout.Application.Mappings;

public sealed class FinishOrderMapping : Profile
{
    public FinishOrderMapping()
    {
        CreateMap<(FinishOrderRequest, Guid), FinishOrderCommand>()
            .ForCtorParam("UserId", x => x.MapFrom(src => src.Item2))
            .ForCtorParam("CardHolderCpf", x => x.MapFrom(src => src.Item1.CardHolderCpf))
            .ForCtorParam("CardHolderName", x => x.MapFrom(src => src.Item1.CardHolderName))
            .ForCtorParam("CardNumber", x => x.MapFrom(src => src.Item1.CardNumber))
            .ForCtorParam("CardExpirationDate", x => x.MapFrom(src => src.Item1.CardExpirationDate))
            .ForCtorParam("CardSecurityCode", x => x.MapFrom(src => src.Item1.CardSecurityCode));
    }
}
