using HomePower.GivEnergy.Service;
using HomePower.MyEnergi.Service;
using HomePower.Orchestrator.Handlers;

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
    ITimeProvider _timeProvider,
    IChargingHandler _firstHandler,
    OrchestratorSettings _settings) : IHomeChargerOrchestrator
{
    /// <inheritdoc/>
    public async Task<bool> UpdateChargingScheduleAsync()
    {
        HandlerContext context = await CreateHandlerContextAsync();

        _firstHandler.Handle(context);

        if (!context.ShouldUpdateChargeTimes)
            return false;

        return await UpdateChargerTimesAsync(context);
    }

    private async Task<HandlerContext> CreateHandlerContextAsync()
    {
        var evChargeStatus = await _myEnergiService.GetEvChargeStatusAsync();

        var context = new HandlerContext
        {
            Settings = _settings,
            EvChargeStatus = evChargeStatus,
            CurrentTime = _timeProvider.GetCurrentTime()
        };

        return context;
    }

    private async Task<bool> UpdateChargerTimesAsync(HandlerContext context)
    {
        if (context.NewChargeStartTime > context.NewChargeEndTime)
        {
            return false;
        }

        await UpdateChargerStartTimeAsync(context);
        await UpdateChargerEndTimeAsync(context);

        return context.ChargeTimesUpdated;
    }

    private async Task UpdateChargerStartTimeAsync(HandlerContext context)
    {
        var currentChargerStartTime = await _givEnergyService.GetBatteryChargeStartTimeAsync();

        if (context.NewChargeStartTime != currentChargerStartTime)
        {
            await _givEnergyService.UpdateBatteryChargeStartTimeAsync(context.NewChargeStartTime);
            context.ChargeTimesUpdated = true;
        }
    }

    private async Task UpdateChargerEndTimeAsync(HandlerContext context)
    {
        var currentChargerEndTime = await _givEnergyService.GetBatteryChargeEndTimeAsync();

        if (context.NewChargeEndTime != currentChargerEndTime)
        {
            await _givEnergyService.UpdateBatteryChargeEndTimeAsync(context.NewChargeEndTime);
            context.ChargeTimesUpdated = true;
        }
    }
}
