namespace HomePower.Orchestrator.Handlers;

internal class NoHandler : IChargingHandler
{
    public static readonly NoHandler Instance = new();

    public int Order => int.MaxValue;

    public void SetNext(IChargingHandler nextHandler)
    {
        // No-op
    }

    public Task HandleAsync(HandlerContext context)
    {
        // No-op
        return Task.CompletedTask;
    }
}