using EM.Checkout.Application.Interfaces.ExternalServices;
using EM.Checkout.Application.Models;
using Newtonsoft.Json;

namespace EM.Checkout.Infraestructure.ExternalServices;

public sealed class CartExternalService : ICartExternalService
{
    private readonly HttpClient _httpClient;

    public CartExternalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CartDTO?> GetItemsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/Carts");
        string responseString = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<CartDTO?>(responseString);
    }
}
