using HomePower.GivEnergy;
using HomePower.GivEnergy.Extensions;
using HomePower.GivEnergy.Settings;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Extensions;
using HomePower.MyEnergi.Settings;
using HomePower.Orchestrator;
using HomePower.Orchestrator.Extensions;
using HomePower.Orchestrator.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// Basic CLI used to trial and demo the services

// Load configuration from user secrets
IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var myEnergiSettings = config.GetSection("MyEnergi").Get<MyEnergiSettings>();
var givEnergySettings = config.GetSection("GivEnergy").Get<GivEnergySettings>();
var orchestratorSettings = config.GetSection("Orchestrator").Get<OrchestratorSettings>();


Demo2(myEnergiSettings!, givEnergySettings!, orchestratorSettings!).Wait();

async Task Demo1(MyEnergiSettings meSettings, GivEnergySettings geSettings)
{
    var services = new ServiceCollection();
    var sp = services
        .AddGivEnergyDependencies(geSettings!)
        .AddMyEnergiDependencies(meSettings!)
        .BuildServiceProvider();

    var givEnergyService = sp.GetService<IGivEnergyService>();
    var acChargeEnabled = await givEnergyService.GetACChargeEnabledAsync();
    var chargeStartTime = await givEnergyService.GetBatteryChargeStartTime1Async();
    var chargeEndTime = await givEnergyService.GetBatteryChargeEndTime1Async();

    Console.WriteLine($"House Battery Charger\n Enabled: {acChargeEnabled}, Start: {chargeStartTime}, End: {chargeEndTime}");

    var myEnergiService = sp.GetService<IMyEnergiService>();

    Console.WriteLine("\nEV Charger Status");
    Console.WriteLine("=================");
    for (int i = 0; i < 10; i++)
    {
        var evChargeStatus = await myEnergiService!.GetEvChargeStatusAsync();
        Console.Write($"\r {DateTime.Now:HH:mm:ss} {evChargeStatus}   ");
        Thread.Sleep(2000);
    }
    Console.WriteLine("\nDone");
}

async Task Demo2(MyEnergiSettings meSettings, GivEnergySettings geSettings, OrchestratorSettings orchestratorSettings)
{
    var services = new ServiceCollection();
    var sp = services
        .AddOrchestratorServices(orchestratorSettings, meSettings, geSettings)
        .BuildServiceProvider();

    var orchestrator = sp.GetService<IHomeChargerOrchestrator>();

    await orchestrator.UpdateChargingScheduleAsync();
}