using HomePower.GivEnergy.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace HomePower.GivEnergy;

/// <summary>
/// Provides extension methods for adding GivEnergy dependencies to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds GivEnergy dependencies to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the dependencies to.</param>
    /// <param name="settings">The settings for configuring the GivEnergy services.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGivEnergyDependencies(this IServiceCollection services, GivEnergySettings settings)
    {
        services.AddSingleton(settings)
            .AddScoped<IGivEnergyService, GivEnergyService>()
            .AddHttpClient<IGivEnergyService, GivEnergyService>(c =>
            {
                c.BaseAddress = new Uri(settings.BaseUrl);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiBearer);
            });

        return services;
    }
}
