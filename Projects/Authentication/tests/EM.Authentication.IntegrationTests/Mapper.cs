using Newtonsoft.Json;
using System.Text;
using WH.SharedKernel.ResourceManagers;

namespace EM.Authentication.IntegrationTests;

internal static class Mapper
{
    public static StringContent MapObjectToStringContent(object request) =>
        new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

    public static async Task<IEnumerable<Error>?> MapHttpResponseMessageToErrors(HttpResponseMessage response)
    {
        string responseString = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Error>>(responseString);
    }
}
