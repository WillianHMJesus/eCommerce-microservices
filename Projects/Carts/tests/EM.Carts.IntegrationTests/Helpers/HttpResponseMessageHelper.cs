using System.Text.Json;

namespace EM.Carts.IntegrationTests.Helpers;

public sealed class HttpResponseMessageHelper
{
    public async Task<T?> DeserializeToObject<T>(HttpResponseMessage message)
    {
        string responseBody = await message.Content.ReadAsStringAsync();
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<T>(responseBody, jsonSerializerOptions);
    }
}
