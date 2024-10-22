using HomePower.MyEnergi.Settings;
using HomePower.MyEnergi.Authentication;
using Microsoft.Extensions.DependencyInjection;
using HomePower.MyEnergi.Client;
using System.Diagnostics.CodeAnalysis;

namespace HomePower.MyEnergi.Extensions;

/// <summary>
/// Provides extension methods for adding MyEnergi dependencies to the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MyEnergi dependencies to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the dependencies to.</param>
    /// <param name="settings">The settings for MyEnergi integration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyEnergiDependencies(this IServiceCollection services, MyEnergiSettings settings)
    {
        services
            .AddSingleton(settings)
            .AddScoped<IMyEnergiClient, MyEnergiClient>()
            .AddScoped<IMyEnergiService, MyEnergiService>()
            .AddHttpClient<IMyEnergiClient, MyEnergiClient>(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .ConfigurePrimaryHttpMessageHandler(() => new DigestAuthHandler(settings.ZappiSerialNumber, settings.ZappiApiKey));
        return services;
    }
}
