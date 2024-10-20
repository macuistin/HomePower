using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;

/// <summary>
/// Handler to check the charging status and update the schedule accordingly.
/// </summary>
public class EvChargingFullPowerPreImmersionHandler : IChargingHandler
{
    public int Order => 4;

    private IChargingHandler _next = NoHandler.Instance;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public async Task HandleAsync(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus == ChargerStatus.Charging
            && context.EvChargeStatus.ChargeRateWatts > context.EvChargeLowPowerCutOffWatts
            && context.CurrentTime < context.PreImmersionTime)
        {
            await context.GivEnergyService.UpdateBatteryChargeStartTimeAsync(context.HouseChargeWindowStart);
            await context.GivEnergyService.UpdateBatteryChargeEndTimeAsync(context.PreImmersionTime);

            return;
        }

        await _next.HandleAsync(context);
    }
}
