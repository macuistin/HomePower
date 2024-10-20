using HomePower.Orchestrator.Handlers;

namespace HomePower.Orchestrator;

internal class ChargingHandlerChainBuilder(IEnumerable<IChargingHandler> _handlers) : IChargingHandlerChainBuilder
{
    public IChargingHandler BuildChain()
    {
        ValidateHandlerChainOrder();

        IChargingHandler currentHandler = NoHandler.Instance;

        foreach (var handler in _handlers
            .OrderByDescending(h => h.Order))
        {
            handler.SetNext(currentHandler);
            currentHandler = handler;
        }

        return currentHandler;
    }

    private void ValidateHandlerChainOrder()
    {
        var duplicateOrders = _handlers.GroupBy(h => h.Order)
                                       .Where(g => g.Count() > 1)
                                       .Select(g => g.Key)
                                       .ToList();

        if (duplicateOrders.Count != 0)
        {
            throw new InvalidOperationException($"Duplicate Order values found in handlers: {string.Join(", ", duplicateOrders)}");
        }
    }
}
