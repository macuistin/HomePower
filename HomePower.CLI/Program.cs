using HomePower.GivEnergi;
using HomePower.GivEnergi.Service;
using HomePower.MyEnergy;
using HomePower.MyEnergy.Service;
using HomePower.MyEnergy.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

var myEnergiSettings = config.GetSection("MyEnergi").Get<MyEnergiSettings>();
var givEnergySettings = config.GetSection("GivEnergy").Get<GivEnergySettings>();

// create an IServiceCollection for dependency injection
var services = new ServiceCollection();
var sp = services
    .AddGivEnergyDependencies(givEnergySettings!)
    .AddMyEnergiDependencies(myEnergiSettings!)
    .BuildServiceProvider();

var myEnergyService = sp.GetService<IMyEnergiService>();
var evChargeStatus = myEnergyService!.GetEvChargeStatus().Result;

var givEnergyService = sp.GetService<IGivEnergyService>();
var acChargeEnabled = givEnergyService.GetACChargeEnabledAsync().Result;
var chargeStartTime = givEnergyService.GetBatteryChargeStartTimeAsync().Result;
var chargeEndTime = givEnergyService.GetBatteryChargeEndTimeAsync().Result;

Console.WriteLine($"Charging Status\n: {evChargeStatus}");
Console.WriteLine($"Houre Battery Charger\n Enabled: {acChargeEnabled}, Start: {chargeStartTime}, End: {chargeEndTime}");
