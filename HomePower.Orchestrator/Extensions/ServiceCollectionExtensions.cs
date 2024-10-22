using HomePower.Orchestrator.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace HomePower.Orchestrator.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the orchestrator services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="settings">Orchestrator settings.</param>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddOrchestratorServices(this IServiceCollection services, OrchestratorSettings settings)
    {
        services
            .AddSingleton(settings)
            .AddTransient<IHomeChargerOrchestrator, HomeChargerOrchestrator>();

        return services;
    }
}
