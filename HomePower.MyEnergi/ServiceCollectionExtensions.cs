using HomePower.MyEnergi.Service;
using HomePower.MyEnergi.Settings;
using HomePower.MyEnergi.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HomePower.MyEnergi;

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