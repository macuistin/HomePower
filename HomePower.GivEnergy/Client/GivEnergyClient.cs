using HomePower.GivEnergy.Dto;
using HomePower.GivEnergy.Settings;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace HomePower.GivEnergy.Client;

/// <summary>
/// Provides methods to interact with GivEnergy services.
/// </summary>
/// <param name="_httpClient">The HTTP client used to make requests to the GivEnergy API.</param>
/// <param name="_settings">The settings for GivEnergy integration.</param>
public class GivEnergyClient(ILogger<GivEnergyClient> _logger, HttpClient _httpClient, GivEnergySettings _settings) : IGivEnergyClient
{
    const string ApiContext = "HomePower automation";

    public async Task<SettingResponseDto<TimeOnly>> GetTimeSettingAsync(InverterSettingId settingId)
    {
        var setting = await GetSettingAsync<string>(settingId);

        if (setting.Success && TimeOnly.TryParse(setting.Data.Value, out var time))
            return SettingResponseDto<TimeOnly>.CreateSuccess(time);

        return SettingResponseDto<TimeOnly>.Failed;
    }

    public async Task<SettingResponseDto<T>> GetSettingAsync<T>(InverterSettingId settingId)
        where T : notnull
        => await GetSettingAsync<T>((int)settingId);

    public async Task<SettingResponseDto<T>> GetSettingAsync<T>(int settingId)
        where T : notnull
    {
        var apiPath = $"/v1/inverter/{_settings.InverterSerialNumber}/settings/{(int)settingId}/read";
        var request = new SettingRequestDto
        {
            Context = ApiContext
        };

        // I did not design this API endpoint.
        // It incorrectly uses a Post and HttpStatusCode.Created to get values
        var response = await _httpClient.PostAsJsonAsync(apiPath, request);
        if (response.StatusCode != System.Net.HttpStatusCode.Created)
            return SettingResponseDto<T>.Failed;

        var responseDto = await response.Content.ReadFromJsonAsync<SettingResponseDto<T>>();

        return responseDto ?? SettingResponseDto<T>.Failed;
    }

    public async Task<bool> UpdateTimeSettingAsync<T>(InverterSettingId settingId, TimeOnly value)
        => await  UpdateSettingAsync(settingId, $"{value:HH:mm}");

    public async Task<bool> UpdateSettingAsync<T>(InverterSettingId settingId, T value)
        where T : notnull
    {
        _logger.LogTrace("Updating setting: {InverterSettingId}, value: {Value}", settingId, value);        
        if (await UpdateSettingAsync((int)settingId, value))
        {
            _logger.LogInformation("Setting updated successfully: {InverterSettingId}, value: {Value}", settingId, value);
            return true;
        }
        _logger.LogWarning("Failed to update setting: {InverterSettingId}, value: {Value}", settingId, value);
        return false;
    }

    private async Task<bool> UpdateSettingAsync<T>(int settingId, T value)
        where T : notnull
    {
        var apiPath = $"/v1/inverter/{_settings.InverterSerialNumber}/settings/{(int)settingId}/write";

        await Task.Delay(100);

        throw new NotImplementedException("I just haven't got to this yet!");

        //return true;
    }
}
