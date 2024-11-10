using AutoMapper;
using EM.Carts.Application.DTOs;
using EM.Carts.Domain.Entities;

namespace EM.Carts.Application.Mappings;

public sealed class CartMapping : Profile
{
    public CartMapping()
    {
        CreateMap<Cart, CartDTO>()
            .ForMember(x => x.TotalValue, opt => opt.Ignore())
            .ForMember(x => x.Items, m => m.MapFrom(src => src.Items));

        CreateMap<Item, ItemDTO>();
    }
}
