using HomePower.GivEnergy.Client;
using HomePower.GivEnergy.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace HomePower.GivEnergy.Extensions;

/// <summary>
/// Provides extension methods for adding GivEnergy dependencies to the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
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
            .AddScoped<IGivEnergyClient, GivEnergyClient>()
            .AddScoped<IGivEnergyService, GivEnergyService>()
            .AddHttpClient<IGivEnergyClient, GivEnergyClient>(c =>
            {
                c.BaseAddress = new Uri(settings.BaseUrl);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiBearer);
            });

        return services;
    }
}
