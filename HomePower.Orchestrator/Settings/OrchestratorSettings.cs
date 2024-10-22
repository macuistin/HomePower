namespace HomePower.Orchestrator.Settings;

public record OrchestratorSettings
{
    // Properties
    public int EvChargeLowPowerCutOffWatts { get; init; }

    public TimeOnly HouseChargeWindowStart { get; init; }
    public TimeOnly HouseChargeWindowEnd { get; init; }

    public TimeOnly ImmersionStart { get; init; }
    public TimeOnly ImmersionEnd { get; init; }

    public int PreImmersionMinutes { get; init; }
    public int PostImmersionMinutes { get; init; }

    public TimeOnly PreImmersionTime => ImmersionStart.AddMinutes(PreImmersionMinutes);
    public TimeOnly PostImmersionTime => ImmersionEnd.AddMinutes(PostImmersionMinutes);
}
