using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;

public class EvChargingFullPowerPreImmersionHandler : IChargingHandler
{
    private IChargingHandler _next = NoHandler.Instance;

    public int Order => 4;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public void Handle(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus == ChargerStatus.Charging
            && context.EvChargeStatus.ChargeRateWatts > context.Settings.EvChargeLowPowerCutOffWatts
            && context.CurrentTime < context.Settings.PreImmersionTime)
        {
            context.SetNewChargeTimes(context.Settings.HouseChargeWindowStart, context.Settings.PreImmersionTime);

            return;
        }

        _next.Handle(context);
    }
}
