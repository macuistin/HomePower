﻿using HomePower.GivEnergy;
using HomePower.GivEnergy.Service;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Service;
using HomePower.MyEnergi.Settings;
using HomePower.Orchestrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// Load configuration from user secrets
IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

var myEnergiSettings = config.GetSection("MyEnergi").Get<MyEnergiSettings>();
var givEnergySettings = config.GetSection("GivEnergy").Get<GivEnergySettings>();

Demo2(myEnergiSettings, givEnergySettings).Wait();

async Task Demo1(MyEnergiSettings meSettings, GivEnergySettings geSettings)
{
    var services = new ServiceCollection();
    var sp = services
        .AddGivEnergyDependencies(geSettings!)
        .AddMyEnergiDependencies(meSettings!)
        .BuildServiceProvider();

    var givEnergyService = sp.GetService<IGivEnergyService>();
    var acChargeEnabled = await givEnergyService.GetACChargeEnabledAsync();
    var chargeStartTime = await givEnergyService.GetBatteryChargeStartTimeAsync();
    var chargeEndTime = await givEnergyService.GetBatteryChargeEndTimeAsync();

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

async Task Demo2(MyEnergiSettings meSettings, GivEnergySettings geSettings)
{
    var services = new ServiceCollection();
    var sp = services
        .AddGivEnergyDependencies(geSettings!)
        .AddMyEnergiDependencies(meSettings!)
        .AddOrchestratorServices()
        .BuildServiceProvider();

    var orchestrator = sp.GetService<IHomeChargerOrchestrator>();

    await orchestrator.UpdateChargingScheduleAsync();
}