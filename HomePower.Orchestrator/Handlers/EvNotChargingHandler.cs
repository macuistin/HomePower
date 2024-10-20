using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator.Handlers;

public class EvNotChargingHandler : IChargingHandler
{
    private IChargingHandler _next = NoHandler.Instance;

    public int Order => 2;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public void Handle(HandlerContext context)
    {
        if (context.EvChargeStatus.ChargerStatus != ChargerStatus.Charging)
        {
            context.SetNewChargeTimes(context.Settings.HouseChargeWindowStart, context.Settings.HouseChargeWindowEnd);

            return;
        }

        _next.Handle(context);
    }
}
