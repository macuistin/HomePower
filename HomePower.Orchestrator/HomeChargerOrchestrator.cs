using HomePower.GivEnergy;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Model;
using HomePower.Orchestrator.Settings;

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
/// <exception cref="InvalidOperationException">Thrown when duplicate Order values are found in handlers.</exception>
public class HomeChargerOrchestrator(
    IGivEnergyService _givEnergyService,
    IMyEnergiService _myEnergiService,
    OrchestratorSettings _settings) : IHomeChargerOrchestrator
{
    /// <inheritdoc/>
    public async Task<bool> UpdateChargingScheduleAsync()
    {
        var evChargeStatus = await _myEnergiService.GetEvChargeStatusAsync();
        
        var highLoadExpectedForImmersion = evChargeStatus?.ChargerStatus == ChargerStatus.Charging
            && (evChargeStatus?.ChargeRateWatts ?? 0) > _settings.EvChargeLowPowerCutOffWatts;
        
        return highLoadExpectedForImmersion
            ? await UpdateChargeSchedulePreAndPostImmersionAsync()
            : await UpdateChargeScheduleToFullWindow();
    }

    private async Task<bool> UpdateChargeScheduleToFullWindow()
    {
        var updated1 = await _givEnergyService.UpdateACCharge1TimesAsync(_settings.HouseChargeWindowStart, _settings.HouseChargeWindowEnd);
        var updated2 = await _givEnergyService.UpdateACCharge2TimesAsync(TimeOnly.MinValue, TimeOnly.MinValue);

        return updated1 || updated2;
    }

    private async Task<bool> UpdateChargeSchedulePreAndPostImmersionAsync()
    {
        var updated1 = await _givEnergyService.UpdateACCharge1TimesAsync(_settings.HouseChargeWindowStart, _settings.PreImmersionTime);
        var updated2 = await _givEnergyService.UpdateACCharge2TimesAsync(_settings.PostImmersionTime, _settings.HouseChargeWindowEnd);

        return updated1 || updated2;
    }
}
