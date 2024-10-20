using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;

public class EvChargingFullPowerPostImmersionHandler : IChargingHandler
{
    private IChargingHandler _next = NoHandler.Instance;

    public int Order => 5;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public void Handle(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus == ChargerStatus.Charging
            && context.EvChargeStatus.ChargeRateWatts > context.Settings.EvChargeLowPowerCutOffWatts
            && context.CurrentTime > context.Settings.ImmersionStart)
        {
            context.SetNewChargeTimes(context.Settings.PostImmersionTime, context.Settings.HouseChargeWindowEnd);

            return;
        }

        _next.Handle(context);
    }
}
