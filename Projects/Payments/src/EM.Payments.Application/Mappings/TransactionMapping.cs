using AutoMapper;
using EM.Common.Core.Events;
using EM.Payments.Domain.Entities;

namespace EM.Payments.Application.Mappings;

public sealed class TransactionMapping : Profile
{
    public TransactionMapping()
    {
        CreateMap<(OrderCreatedEvent, bool), Transaction>()
            .ForCtorParam("userId", x => x.MapFrom(src => src.Item1.UserId))
            .ForCtorParam("orderId", x => x.MapFrom(src => src.Item1.OrderId))
            .ForCtorParam("value", x => x.MapFrom(src => src.Item1.Value))
            .ForCtorParam("cardNumber", x => x.MapFrom(src => MaskCardNumber(src.Item1.CardNumber)))
            .ForCtorParam("status", x => x.MapFrom(src => src.Item2 ? "Paid" : "PaymentRefused"))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore())
            .ForMember(x => x.Date, opt => opt.Ignore());
    }

    public static string MaskCardNumber(string cardNumber)
    {
        cardNumber = cardNumber.Replace(" ", "");
        string mask = new string('*', cardNumber.Length - 4);

        return mask + cardNumber.Substring(cardNumber.Length - 4, 4);
    }
}
