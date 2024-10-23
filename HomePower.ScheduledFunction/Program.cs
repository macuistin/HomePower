using HomePower.GivEnergy.Settings;
using HomePower.MyEnergi.Settings;
using HomePower.Orchestrator.Extensions;
using HomePower.Orchestrator.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        // Load settings from 'local.settings.json' in local development
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables(); // Load settings from Azure in production
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostContext, services) =>
    {
        var orchestrationSettings = hostContext.Configuration.GetSection("Orchestrator").Get<OrchestratorSettings>();
        var myEnergiSettings = hostContext.Configuration.GetSection("MyEnergi").Get<MyEnergiSettings>();
        var givEnergySettings = hostContext.Configuration.GetSection("GivEnergy").Get<GivEnergySettings>();

        services.AddApplicationInsightsTelemetryWorkerService()
                .ConfigureFunctionsApplicationInsights()
                .AddOrchestratorServices(orchestrationSettings, myEnergiSettings, givEnergySettings);
    })
    .Build();

host.Run();
