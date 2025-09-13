using Newtonsoft.Json;
using System.Text;
using WH.SharedKernel.ResourceManagers;

namespace EM.Authentication.IntegrationTests.Fixtures;

public class BaseFixture
{
    public StringContent MapObjectToStringContent(object request) =>
        new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

    public async Task<IEnumerable<Error>?> MapHttpResponseMessageToErrors(HttpResponseMessage response)
    {
        string responseString = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Error>>(responseString);
    }
}
