using HomePower.GivEnergy.Extensions;
using HomePower.GivEnergy.Settings;
using HomePower.MyEnergi.Extensions;
using HomePower.MyEnergi.Settings;
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
    public static IServiceCollection AddOrchestratorServices(
        this IServiceCollection services, 
        OrchestratorSettings orchestratorSettings,
        MyEnergiSettings meSettings, 
        GivEnergySettings geSettings)
    {
        
        services
            .AddSingleton(orchestratorSettings)
            .AddMyEnergiDependencies(meSettings)
            .AddGivEnergyDependencies(geSettings)
            .AddTransient<IHomeChargerOrchestrator, HomeChargerOrchestrator>();

        return services;
    }
}
