namespace HomePower.Orchestrator.Handlers;

/// <summary>
/// Handler to check if the current time is within the allowed range.
/// </summary>
public class TimeRangeHandler : IChargingHandler
{
    public int Order => 1;

    private IChargingHandler _next = NoHandler.Instance;

    public void SetNext(IChargingHandler nextHandler)
    {
        _next = nextHandler;
    }

    public async Task HandleAsync(HandlerContext context)
    {
        if (context.CurrentTime < context.HouseChargeWindowStart || context.CurrentTime >= context.HouseChargeWindowEnd)
        {
            // Do nothing
            return;
        }

        await _next.HandleAsync(context);
    }
}
