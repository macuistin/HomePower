namespace HomePower.Orchestrator.Handlers;

/// <summary>
/// Handler to check if the current time is within the allowed range.
/// </summary>
public class TimeRangeHandler : IChargingHandler
{
    private IChargingHandler _next = NoHandler.Instance;

    public int Order => 1;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public void Handle(HandlerContext context)
    {
        if (context.CurrentTime < context.Settings.HouseChargeWindowStart || context.CurrentTime >= context.Settings.HouseChargeWindowEnd)
        {
            // Do nothing
            return;
        }

        _next.Handle(context);
    }
}
