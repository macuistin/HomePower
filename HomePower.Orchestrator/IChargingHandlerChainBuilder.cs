using HomePower.Orchestrator.Handlers;

namespace HomePower.Orchestrator;

public interface IChargingHandlerChainBuilder
{
    IChargingHandler BuildChain();
}