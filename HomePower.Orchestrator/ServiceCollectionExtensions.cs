using HomePower.Orchestrator.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HomePower.Orchestrator;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the orchestrator services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    public static IServiceCollection AddOrchestratorServices(this IServiceCollection services)
    {
        // Register all IChargingHandler implementations from the current assembly
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypes = assembly.GetTypes()
            .Where(t => typeof(IChargingHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var handlerType in handlerTypes)
        {
            services.AddTransient(typeof(IChargingHandler), handlerType);
        }

        services.AddSingleton<ITimeProvider, TimeProvider>();
        services.AddTransient<IHomeChargerOrchestrator, HomeChargerOrchestrator>();

        return services;
    }
}
