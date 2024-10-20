using HomePower.GivEnergy.Service;
using HomePower.MyEnergi.Service;
using HomePower.Orchestrator.Handlers;

namespace HomePower.Orchestrator;

/// <summary>
/// Orchestrates the home charger operations.
/// </summary>
public class HomeChargerOrchestrator : IHomeChargerOrchestrator
{
    private readonly IGivEnergyService _givEnergyService;
    private readonly IMyEnergiService _myEnergiService;
    private readonly ITimeProvider _timeProvider;
    private readonly IEnumerable<IChargingHandler> _handlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeChargerOrchestrator"/> class.
    /// </summary>
    /// <param name="givEnergyService">The GivEnergy service.</param>
    /// <param name="myEnergiService">The MyEnergi service.</param>
    /// <param name="handlers">The collection of charging handlers.</param>
    /// <exception cref="InvalidOperationException">Thrown when duplicate Order values are found in handlers.</exception>
    public HomeChargerOrchestrator(IGivEnergyService givEnergyService, IMyEnergiService myEnergiService, ITimeProvider timeProvider, IEnumerable<IChargingHandler> handlers)
    {
        _givEnergyService = givEnergyService;
        _myEnergiService = myEnergiService;
        _timeProvider = timeProvider;
        _handlers = handlers;

        // Validate that handlers have unique Order values
        var duplicateOrders = _handlers.GroupBy(h => h.Order)
                                       .Where(g => g.Count() > 1)
                                       .Select(g => g.Key)
                                       .ToList();

        if (duplicateOrders.Count != 0)
        {
            throw new InvalidOperationException($"Duplicate Order values found in handlers: {string.Join(", ", duplicateOrders)}");
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateChargingScheduleAsync()
    {
        HandlerContext context = await CreateHandlerContext();

        var currentHandler = GetFirstHandlerInChain();

        if (currentHandler == NoHandler.Instance)
            return false;

        await currentHandler.HandleAsync(context);

        return true;
    }

    private async Task<HandlerContext> CreateHandlerContext()
    {
        var evChargeStatus = await _myEnergiService.GetEvChargeStatusAsync();

        var context = new HandlerContext
        {
            EvChargeStatus = evChargeStatus,
            GivEnergyService = _givEnergyService,
            MyEnergiService = _myEnergiService,
            CurrentTime = _timeProvider.GetCurrentTime(),
        };
        return context;
    }

    /// <summary>
    /// Gets the first handler in the chain of responsibility.
    /// </summary>
    /// <returns>The first handler in the chain.</returns>
    private IChargingHandler GetFirstHandlerInChain()
    {
        IChargingHandler currentHandler = NoHandler.Instance;

        foreach (var handler in _handlers.OrderBy(h => h.Order).Reverse())
        {
            handler.SetNext(currentHandler);
            currentHandler = handler;
        }

        return currentHandler;
    }
}
