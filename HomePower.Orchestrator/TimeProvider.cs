namespace HomePower.Orchestrator;

public class TimeProvider : ITimeProvider
{

    /// <inheritdoc/>
    public TimeOnly GetCurrentTime() => TimeOnly.FromDateTime(DateTime.Now);
}
