namespace HomePower.Orchestrator.Handlers;

public class NoHandler : IChargingHandler
{
    public static readonly NoHandler Instance = new();

    public int Order => int.MaxValue;

    public void SetNext(IChargingHandler nextHandler)
    {
        // Do nothing
    }

    public void Handle(HandlerContext context)
    {
        // Do nothing
    }
}