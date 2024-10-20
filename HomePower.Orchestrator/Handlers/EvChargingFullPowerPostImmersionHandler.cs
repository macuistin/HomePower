﻿using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;
internal class EvChargingFullPowerPostImmersionHandler : IChargingHandler
{
    public int Order => 5;

    private IChargingHandler _next = NoHandler.Instance;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public async Task HandleAsync(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus == ChargerStatus.Charging
            && context.EvChargeStatus.ChargeRateWatts > context.EvChargeLowPowerCutOffWatts
            && context.CurrentTime > context.ImmersionStart)
        {
            await context.GivEnergyService.UpdateBatteryChargeStartTimeAsync(context.PostImmersionTime);
            await context.GivEnergyService.UpdateBatteryChargeEndTimeAsync(context.HouseChargeWindowEnd);

            return;
        }

        await _next.HandleAsync(context);
    }
}
