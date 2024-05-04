using EM.Checkout.Application.DTOs;
using EM.Checkout.Application.Interfaces;
using Newtonsoft.Json;

namespace EM.Checkout.Infraestructure.ExternalServices;

public sealed class CartExternalService : ICartExternalService
{
    private readonly IHttpClientFactory _clientFactory;

    public CartExternalService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<ItemDTO>> GetItemsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        HttpClient client = _clientFactory.CreateClient("Cart");
        HttpResponseMessage response = await client.GetAsync("Carts");

        response.EnsureSuccessStatusCode();

        return JsonConvert.DeserializeObject<List<ItemDTO>>(await response.Content.ReadAsStringAsync())
            ?? (Enumerable.Empty<ItemDTO>()).ToList();
    }
}
