using HomePower.MyEnergy.Dto;
using HomePower.MyEnergy.Model;
using System.Net.Http.Json;

namespace HomePower.MyEnergy.Service;

public class MyEnergiService(HttpClient httpClient) : IMyEnergiService
{
    private readonly HttpClient _httpClient = httpClient;

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

    public async Task<EvChargeStatus> GetEvChargeStatus()
    {
        var zappiStatus = await GetZappiStatusAsync();

        if (zappiStatus.Success)
            return zappiStatus.Zappi.ToEvChargeStatus();

        return EvChargeStatus.Failed;
    }
}
