using AutoMapper;
using EM.Carts.Application.DTOs;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Domain.Entities;

namespace EM.Carts.Application.Mappings;

public sealed class ItemMapping : Profile
{
    public ItemMapping()
    {
        CreateMap<Item, ItemDTO>()
            .ForMember(x => x.TotalValue, opt => opt.Ignore());

        CreateMap<(AddItemRequest, ProductDTO), Item>()
            .ForCtorParam("productId", x => x.MapFrom(src => src.Item1.ProductId))
            .ForCtorParam("productName", x => x.MapFrom(src => src.Item2.Name))
            .ForCtorParam("productImage", x => x.MapFrom(src => src.Item2.Image))
            .ForCtorParam("value", x => x.MapFrom(src => src.Item2.Value))
            .ForCtorParam("quantity", x => x.MapFrom(src => src.Item1.Quantity))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore());
    }
}
