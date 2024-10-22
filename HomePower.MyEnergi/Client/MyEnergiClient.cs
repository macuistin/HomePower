using HomePower.MyEnergi.Dto;
using System.Net.Http.Json;

namespace HomePower.MyEnergi.Client;

/// <summary>
/// Provides methods to interact with MyEnergi services.
/// </summary>
/// <param name="httpClient">The HTTP client used to make requests to the MyEnergi API.</param>
public class MyEnergiClient(HttpClient _httpClient) : IMyEnergiClient
{
    /// <inheritdoc/>
    public async Task<ZappiStatusResult> GetZappiStatusAsync()
    {
        var response = await _httpClient.GetAsync("/cgi-jstatus-Z");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return ZappiStatusResult.Failed;

        var responseDto = await response.Content.ReadFromJsonAsync<ZappiStatusDto>();

        if (responseDto?.Zappi == null || responseDto.Zappi.Count == 0)
            return ZappiStatusResult.Failed;

        return ZappiStatusResult.CreateSuccess(responseDto.Zappi.First());
    }
}
