using EM.Carts.Application;
using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Application.UseCases.RemoveItemQuantity;
using EM.Carts.Application.Validations;
using EM.Carts.Domain.Interfaces;
using EM.Carts.Infraestructure.ExternalServices;
using EM.Carts.Infraestructure.Persistense;
using FluentValidation;
using MongoDB.Driver;


namespace EM.Carts.API;

public static class Extensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMongoClient>(provider =>
        {
            return new MongoClient(configuration.GetConnectionString("mongoDb"));
        });

        services.AddScoped<CartContext>();

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddAutoMapper(AssemblyReference.Assembly);
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped<IGenericValidations, GenericValidations>();

        services.AddScoped<IUseCase<AddItemRequest>, AddItemUseCase>();
        services.AddScoped<IUseCase<AddItemQuantityRequest>, AddItemQuantityUseCase>();
        services.AddScoped<IUseCase<RemoveItemQuantityRequest>, RemoveItemQuantityUseCase>();
        services.AddScoped<IUseCase<DeleteItemRequest>, DeleteItemUseCase>();
        services.AddScoped<IUseCase<DeleteAllItemsRequest>, DeleteAllItemsUseCase>();
        services.AddScoped<IUseCase<GetCartByUserIdRequest>, GetCartByUserIdUseCase>();
        services.Decorate(typeof(IUseCase<>), typeof(UseCaseValidation<>));

        services.AddHttpClient<ICatalogExternalService, CatalogExternalService>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("CatalogExternalService:BaseAddress") 
                ?? throw new ArgumentNullException());
        });

        return services;
    }
}
