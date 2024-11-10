using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;
using Newtonsoft.Json;

namespace EM.Carts.Infraestructure.ExternalServices;

public sealed class CatalogExternalService : ICatalogExternalService
{
    private readonly HttpClient _httpClient;

    public CatalogExternalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDTO?> GetProductsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/products/{id}");
        string responseString = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ProductDTO?>(responseString);
    }
}
