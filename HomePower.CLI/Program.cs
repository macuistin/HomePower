using HomePower.GivEnergy;
using HomePower.GivEnergy.Service;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Service;
using HomePower.MyEnergi.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// Load configuration from user secrets
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

var givEnergyService = sp.GetService<IGivEnergyService>();
var acChargeEnabled = givEnergyService.GetACChargeEnabledAsync().Result;
var chargeStartTime = givEnergyService.GetBatteryChargeStartTimeAsync().Result;
var chargeEndTime = givEnergyService.GetBatteryChargeEndTimeAsync().Result;

var myEnergiService = sp.GetService<IMyEnergiService>();
var evChargeStatus = myEnergiService!.GetEvChargeStatus().Result;

Console.WriteLine($"Charging Status\n: {evChargeStatus}");
Console.WriteLine($"House Battery Charger\n Enabled: {acChargeEnabled}, Start: {chargeStartTime}, End: {chargeEndTime}");
