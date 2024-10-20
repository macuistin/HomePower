using HomePower.GivEnergy.Service;
using HomePower.MyEnergi.Model;
using HomePower.MyEnergi.Service;
using System.Globalization;

namespace HomePower.Orchestrator.Handlers;

/// <summary>
/// Represents the context for handling charging requests.
/// </summary>
public class HandlerContext
{
    // TODO: Move these settings to a configuration file.
    public int EvChargeLowPowerCutOffWatts => 3000;
    public TimeOnly ImmersionStart => TimeOnly.Parse("06:25", CultureInfo.InvariantCulture);
    public TimeOnly ImmersionEnd => TimeOnly.Parse("07:10", CultureInfo.InvariantCulture);

    public TimeOnly HouseChargeWindowStart => TimeOnly.Parse("01:00", CultureInfo.InvariantCulture);
    public TimeOnly HouseChargeWindowEnd => TimeOnly.Parse("08:00", CultureInfo.InvariantCulture);

    public TimeOnly PreImmersionTime => ImmersionStart.AddMinutes(-3);
    public TimeOnly PostImmersionTime => ImmersionEnd.AddMinutes(1);

    public required IGivEnergyService GivEnergyService { get; init; } 
    public required IMyEnergiService MyEnergiService { get; init; } 
    public required EvChargeStatus EvChargeStatus { get; init; }

    public required TimeOnly CurrentTime { get; init; }
}
