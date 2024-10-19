using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;
using System.Net.Http.Json;

namespace HomePower.MyEnergi.Service;

/// <summary>
/// Provides methods to interact with MyEnergi services.
/// </summary>
/// <param name="httpClient">The HTTP client used to make requests to the MyEnergi API.</param>
public class MyEnergiService(HttpClient _httpClient) : IMyEnergiService
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

    /// <inheritdoc/>
    public async Task<EvChargeStatus> GetEvChargeStatus()
    {
        var zappiStatus = await GetZappiStatusAsync();

        if (zappiStatus.Success)
            return zappiStatus.Zappi.ToEvChargeStatus();

        return EvChargeStatus.Failed;
    }
}
