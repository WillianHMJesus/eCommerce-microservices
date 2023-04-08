using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItem.Validations;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Domain.Interfaces;
using EM.Carts.Infraestructure.Configurations;
using EM.Carts.Infraestructure.Repositories;

namespace EM.Carts.API;

public static class Extensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CartDatabaseSettings>(configuration.GetSection("CartDatabase"));
        services.AddSingleton<MongoDbConfiguration>();

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddScoped<IAddItemUseCase, AddItemUseCase>();
        services.Decorate<IAddItemUseCase, AddItemRequestValidation>();
        services.AddScoped<IGetCartByUserIdUseCase, GetCartByUserIdUseCase>();

        return services;
    }
}
