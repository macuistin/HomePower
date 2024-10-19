﻿using HomePower.MyEnergi.Service;
using HomePower.MyEnergi.Settings;
using HomePower.MyEnergi.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HomePower.MyEnergi;

/// <summary>
/// Provides extension methods for adding MyEnergi dependencies to the service collection.
/// </summary>
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
        services.AddSingleton(settings);
        services.AddScoped<IMyEnergiService, MyEnergiService>()
            .AddHttpClient<IMyEnergiService, MyEnergiService>(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .ConfigurePrimaryHttpMessageHandler(() => new DigestAuthHandler(settings.ZappiSerialNumber, settings.ZappiApiKey));
        return services;
    }
}
