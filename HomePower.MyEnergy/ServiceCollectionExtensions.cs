using HomePower.MyEnergy.Authentication;
using HomePower.MyEnergy.Service;
using HomePower.MyEnergy.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace HomePower.MyEnergy;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMyEnergiDependencies(this IServiceCollection services, MyEnergiSettings settings)
    {
        services.AddSingleton(settings);
        services.AddScoped<IMyEnergiService, MyEnergiService>()
            .AddHttpClient<IMyEnergiService, MyEnergiService>(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .ConfigurePrimaryHttpMessageHandler(() => new DigestAuthHandler(settings.ZappiSerialNumber, settings.ZappiApiKey));
        return services;
    }
}