﻿using HomePower.GivEnergy.Dto;
using System.Net.Http.Json;
using System.Text.Json;

namespace HomePower.GivEnergy.Service;

internal enum SettingId
{
    ACCharge1StartTime = 64,   // string, time, HH:mm
    ACCharge1EndTime = 65,     // string, time, HH:mm
    ACChargeEnable = 66       // boolean
}

/// <summary>
/// Provides methods to interact with GivEnergy services.
/// </summary>
/// <param name="_httpClient">The HTTP client used to make requests to the GivEnergy API.</param>
/// <param name="_settings">The settings for GivEnergy integration.</param>
public class GivEnergyService(HttpClient _httpClient, GivEnergySettings _settings) : IGivEnergyService
{
    const string ApiContext = "HomePower automation";

    private async Task<SettingResponseDto<T>> GetSettingAsync<T>(SettingId settingId) 
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

    private async Task<bool> UpdateSettingAsync<T>(SettingId settingId, T value)
        where T : notnull
    {
        var apiPath = $"/v1/inverter/{_settings.InverterSerialNumber}/settings/{(int)settingId}/write";

        await Task.Delay(100);

        throw new System.NotImplementedException("I just haven't got to this yet!");

        //return true;
    }

    /// <inheritdoc/>
    public async Task<string> GetBatteryChargeStartTimeAsync()
    {
        var result = await GetSettingAsync<string>(SettingId.ACCharge1StartTime);

        // TODO: This is a PoC, we should handle this better
        if (result == SettingResponseDto<string>.Failed)
            return string.Empty;

        return result.Data.Value;
    }

    /// <inheritdoc/>
    public async Task<string> GetBatteryChargeEndTimeAsync()
    {
        var result = await GetSettingAsync<string>(SettingId.ACCharge1EndTime);

        // TODO: This is a PoC, we should handle this better
        if (result == SettingResponseDto<string>.Failed)
            return string.Empty;

        return result.Data.Value;
    }

    /// <inheritdoc/>
    public async Task<bool> GetACChargeEnabledAsync()
    {
        var result = await GetSettingAsync<bool>(SettingId.ACChargeEnable);

        // TODO: This is a PoC, we should handle this better
        if (result == SettingResponseDto<bool>.Failed)
            return false;

        return result?.Data?.Value ?? false;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateBatteryChargeStartTimeAsync(int hour, int minute)
    {
        if (hour < 0 || hour > 23)
            return false;
        if (minute < 0 || minute > 59)
            return false;

        return await UpdateSettingAsync<string>(SettingId.ACCharge1StartTime, $"{hour:00}:{minute:00}");
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateBatteryChargeEndTimeAsync(int hour, int minute)
    {
        if (hour < 0 || hour > 23)
            return false;
        if (minute < 0 || minute > 59)
            return false;

        return await UpdateSettingAsync<string>(SettingId.ACCharge1EndTime, $"{hour:00}:{minute:00}");
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateACChargeEnabledAsync(bool enabled)
    {
        return await UpdateSettingAsync<bool>(SettingId.ACChargeEnable, enabled);
    }
}
