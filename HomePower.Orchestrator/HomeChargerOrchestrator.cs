using HomePower.GivEnergy;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Model;
using HomePower.Orchestrator.Settings;
using Microsoft.Extensions.Logging;

namespace HomePower.Orchestrator;

/// <summary>
/// Orchestrates the home charger operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HomeChargerOrchestrator"/> class.
/// </remarks>
/// <param name="_givEnergyService">The GivEnergy service.</param>
/// <param name="_myEnergiService">The MyEnergi service.</param>
/// <param name="_firstHandler">First handler in rule chain.</param>
/// <param name="_logger">Logger</param>
public class HomeChargerOrchestrator(
    IGivEnergyService _givEnergyService,
    IMyEnergiService _myEnergiService,
    OrchestratorSettings _settings,
    ILogger<HomeChargerOrchestrator> _logger) : IHomeChargerOrchestrator
{
    /// <inheritdoc/>
    public async Task<bool> UpdateChargingScheduleAsync()
    {
        var evChargeStatus = await _myEnergiService.GetEvChargeStatusAsync();
        
        var highLoadExpectedForImmersion = evChargeStatus?.ChargerStatus == ChargerStatus.Charging
            && (evChargeStatus?.ChargeRateWatts ?? 0) > _settings.EvChargeLowPowerCutOffWatts;

        if (highLoadExpectedForImmersion)
        {
            return await UpdateChargeSchedules(
                (_settings.HouseChargeWindowStart, _settings.PreImmersionTime),
                (_settings.PostImmersionTime, _settings.HouseChargeWindowEnd));
        }

        return await UpdateChargeSchedules(
            (_settings.HouseChargeWindowStart, _settings.HouseChargeWindowEnd));
    }

    private async Task<bool> UpdateChargeSchedules(params (TimeOnly, TimeOnly)[] schedules)
    {
        _logger.LogTrace("Updating charge schedules: {Schedules}", schedules);

        if (schedules.Length == 0)
        {
            _logger.LogWarning("No charge schedules provided");
            return false;
        }

        if (schedules.Length > 2)
        {
            _logger.LogWarning("Only two charge schedules are supported");
            return false;
        }

        var updatedSchedule1 = await _givEnergyService.UpdateACCharge1TimesAsync(schedules[0].Item1, schedules[0].Item2);

        bool updatedSchedule2;
        if (updatedSchedule1 && schedules.Length == 1)
        {
            updatedSchedule2 = await _givEnergyService.UpdateACCharge2TimesAsync(TimeOnly.MinValue, TimeOnly.MinValue);
        }
        else
        {
            updatedSchedule2 = await _givEnergyService.UpdateACCharge2TimesAsync(schedules[1].Item1, schedules[1].Item2);
        }

        var success = updatedSchedule1 || updatedSchedule2;
        if (success)
        {
            _logger.LogInformation("Charge schedule updated: {Schedules}", schedules);
        }
        else
        {
            _logger.LogWarning("Failed to update charge schedule: {Schedules}", schedules);
        }
        return success;
    }
}
