using HomePower.GivEnergy.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace HomePower.GivEnergy;

public static class ServiceCollectionExtensions
{
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
