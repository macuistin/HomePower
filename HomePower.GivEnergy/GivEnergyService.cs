﻿using HomePower.GivEnergy.Client;
using Microsoft.Extensions.Logging;

namespace HomePower.GivEnergy;

public class GivEnergyService(IGivEnergyClient _client) : IGivEnergyService
{
    /// <inheritdoc/>
    public async Task<bool> GetACChargeEnabledAsync()
    {
        var result = await _client.GetSettingAsync<bool>(InverterSettingId.ACChargeEnable);

        // TODO: This is a PoC, we should handle this better

        return result.Success && result.Data.Value;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeStartTime1Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1StartTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeEndTime1Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1EndTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeStartTime2Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2StartTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeEndTime2Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2EndTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeStartTime3Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3StartTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<TimeOnly> GetBatteryChargeEndTime3Async()
    {
        var result = await _client.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3EndTime);

        // TODO: This is a PoC, we should handle this better

        return result.Success ? result.Data.Value : default;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateACChargeEnabledAsync(bool enabled)
    {
        return await _client.UpdateSettingAsync(InverterSettingId.ACChargeEnable, enabled);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateACCharge1TimesAsync(TimeOnly startTime, TimeOnly endTime)
    {
        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge1StartTime, startTime))
        {
            return false;
        }

        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge1EndTime, endTime))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateACCharge2TimesAsync(TimeOnly startTime, TimeOnly endTime)
    {
        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge2StartTime, startTime))
        {
            return false;
        }

        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge2EndTime, endTime))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateACCharge3TimesAsync(TimeOnly startTime, TimeOnly endTime)
    {
        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge3StartTime, startTime))
        {
            return false;
        }

        if (!await _client.UpdateSettingAsync(InverterSettingId.ACCharge3EndTime, endTime))
        {
            return false;
        }

        return true;
    }
}
