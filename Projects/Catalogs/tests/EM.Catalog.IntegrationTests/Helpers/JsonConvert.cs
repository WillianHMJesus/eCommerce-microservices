using System.Text.Json;

namespace EM.Catalog.IntegrationTests.Helpers;

public sealed class JsonConvert
{
    public static T? DeserializeToObject<T>(string json)
    {
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }
}
