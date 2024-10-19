using HomePower.GivEnergi.Dto;
using System.Net.Http.Json;

namespace HomePower.GivEnergi.Service;

internal enum SettingId
{
    ACCharge1StartTime = 64,   // string, time, HH:mm
    ACCharge1EndTime = 65,     // string, time, HH:mm
    ACChargeEnable = 66       // boolean
}

public class GivEnergyService(HttpClient _httpClient, GivEnergySettings _settings) : IGivEnergyService
{
    const string ApiContext = "HomePower automation";

    private async Task<SettingResponseDto> GetSettingAsync(SettingId settingId)
    {
        var apiPath = $"/inverter/{_settings.InverterSerialNumber}/settings/{(int)settingId}/read";
        var request = new SettingRequestDto
        {
            Id = (int)settingId,
            Context = ApiContext
        };
        var response = await _httpClient.PostAsJsonAsync(apiPath, request);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return SettingResponseDto.Failed;

        var responseDto = await response.Content.ReadFromJsonAsync<SettingResponseDto>();

        return responseDto ?? SettingResponseDto.Failed;
    }

    private async Task<bool> UpdateSettingAsync(SettingId settingId, string value)
    {
        var apiPath = $"/inverter/{_settings.InverterSerialNumber}/settings/{(int)settingId}/write";
        await Task.Delay(100);

        return true;
    }

    public async Task<string> GetBatteryChargeStartTimeAsync()
    {
        var result = await GetSettingAsync(SettingId.ACCharge1StartTime);

        // TODO: This is a PoC, we should handle this better
        if (result == SettingResponseDto.Failed)
            return string.Empty;

        return result.Data.Value;
    }

    public async Task<string> GetBatteryChargeEndTimeAsync()
    {
        var result = await GetSettingAsync(SettingId.ACCharge1EndTime);

        // TODO: This is a PoC, we should handle this better
        if (result == SettingResponseDto.Failed)
            return string.Empty;

        return result.Data.Value;
    }

    public async Task<bool> GetACChargeEnabledAsync()
    {
        var result = await GetSettingAsync(SettingId.ACChargeEnable);

        if (result == SettingResponseDto.Failed)
            return false;

        // TODO: This is a PoC, we should handle this better
        var success = bool.TryParse(result.Data.Value, out var value);

        return success && value;
    }

    public async Task<bool> UpdateBatteryChargeStartTimeAsync(int hour, int minute)
    {
        if (hour < 0 || hour > 23)
            return false;
        if (minute < 0 || minute > 59)
            return false;

        return await UpdateSettingAsync(SettingId.ACCharge1StartTime, $"{hour:00}:{minute:00}");
    }

    public async Task<bool> UpdateBatteryChargeEndTimeAsync(int hour, int minute)
    {
        if (hour < 0 || hour > 23)
            return false;
        if (minute < 0 || minute > 59)
            return false;

        return await UpdateSettingAsync(SettingId.ACCharge1EndTime, $"{hour:00}:{minute:00}");
    }

    public async Task<bool> UpdateACChargeEnabledAsync(bool enabled)
    {
        return await UpdateSettingAsync(SettingId.ACChargeEnable, enabled.ToString());
    }
}
