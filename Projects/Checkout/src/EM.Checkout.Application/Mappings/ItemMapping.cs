using AutoMapper;
using EM.Checkout.Application.Models;
using EM.Checkout.Application.Orders.Commands.FinishOrder;
using EM.Checkout.Domain.Entities;
using EM.Common.Core.Events;

namespace EM.Checkout.Application.Mappings;

public sealed class ItemMapping : Profile
{
    public ItemMapping()
    {
        CreateMap<ItemDTO, Item>()
            .ForCtorParam("productId", x => x.MapFrom(src => src.ProductId))
            .ForCtorParam("productName", x => x.MapFrom(src => src.ProductName))
            .ForCtorParam("productImage", x => x.MapFrom(src => src.ProductImage))
            .ForCtorParam("quantity", x => x.MapFrom(src => src.Quantity))
            .ForCtorParam("value", x => x.MapFrom(src => src.Value))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore())
            .ForMember(x => x.Amount, opt => opt.Ignore());

        CreateMap<(Order, FinishOrderCommand), OrderCreatedEvent>()
            .ForCtorParam("UserId", x => x.MapFrom(src => src.Item1.UserId))
            .ForCtorParam("OrderId", x => x.MapFrom(src => src.Item1.Id))
            .ForCtorParam("Value", x => x.MapFrom(src => src.Item1.Amount))
            .ForCtorParam("CardHolderCpf", x => x.MapFrom(src => src.Item2.CardHolderCpf))
            .ForCtorParam("CardHolderName", x => x.MapFrom(src => src.Item2.CardHolderName))
            .ForCtorParam("CardNumber", x => x.MapFrom(src => src.Item2.CardNumber))
            .ForCtorParam("CardExpirationDate", x => x.MapFrom(src => src.Item2.CardExpirationDate))
            .ForCtorParam("CardSecurityCode", x => x.MapFrom(src => src.Item2.CardSecurityCode));
    }
}
