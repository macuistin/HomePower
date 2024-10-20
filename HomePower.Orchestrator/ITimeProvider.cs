
namespace HomePower.Orchestrator;

/// <summary>
/// Provides the current time.
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Gets the current time as a <see cref="TimeOnly"/> value.
    /// </summary>
    TimeOnly GetCurrentTime();
}
