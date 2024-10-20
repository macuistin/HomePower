namespace HomePower.Orchestrator.Handlers;

/// <summary>
/// Defines the interface for handling charging requests.
/// </summary>
public interface IChargingHandler
{
    /// <summary>
    /// Gets the order of the handler in the chain.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Sets the next handler in the chain.
    /// </summary>
    /// <param name="nextHandler">The next handler.</param>
    void SetNext(IChargingHandler nextHandler);

    /// <summary>
    /// Handles the charging request.
    /// </summary>
    /// <param name="context">The context containing the necessary data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(HandlerContext context);
}

