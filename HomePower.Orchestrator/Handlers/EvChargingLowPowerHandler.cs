using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;
internal class EvChargingLowPowerHandler : IChargingHandler
{
    public int Order => 3;

    private IChargingHandler _next = NoHandler.Instance;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public async Task HandleAsync(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus == ChargerStatus.Charging
            && context.EvChargeStatus.ChargeRateWatts < context.EvChargeLowPowerCutOffWatts
            && context.CurrentTime < context.PreImmersionTime)
        {
            await context.GivEnergyService.UpdateBatteryChargeStartTimeAsync(context.HouseChargeWindowStart.Hour, context.HouseChargeWindowStart.Minute);
            await context.GivEnergyService.UpdateBatteryChargeEndTimeAsync(context.HouseChargeWindowEnd.Hour, context.HouseChargeWindowEnd.Minute);

            return;
        }

        await _next.HandleAsync(context);
    }
}
