using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItem.Validations;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.AddItemQuantity.Validations;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.DeleteItem.Validations;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Application.UseCases.SubtractItemQuantity;
using EM.Carts.Application.UseCases.SubtractItemQuantity.Validations;
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
        services.AddScoped<IAddItemQuantityUseCase, AddItemQuantityUseCase>();
        services.Decorate<IAddItemQuantityUseCase, AddItemQuantityRequestValidation>();
        services.AddScoped<ISubtractItemQuantityUseCase, SubtractItemQuantityUseCase>();
        services.Decorate<ISubtractItemQuantityUseCase, SubtractItemQuantityRequestValidation>();
        services.AddScoped<IDeleteItemUseCase, DeleteItemUseCase>();
        services.Decorate<IDeleteItemUseCase, DeleteItemRequestValidation>();
        services.AddScoped<IDeleteAllItemsUseCase, DeleteAllItemsUseCase>();
        services.AddScoped<IGetCartByUserIdUseCase, GetCartByUserIdUseCase>();

        return services;
    }
}
