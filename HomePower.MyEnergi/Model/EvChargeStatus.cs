namespace HomePower.MyEnergi.Model;

/// <summary>
/// Represents the status of Zappi devices.
/// </summary>
public record EvChargeStatus
{
    /// <summary>
    /// Represents a failed status.
    /// </summary>
    public static readonly EvChargeStatus Failed = new();

    /// <summary>
    /// Gets or sets the charge added in KWh.
    /// </summary>
    public double ChargeAddedKWh { get; set; }

    /// <summary>
    /// Gets or sets the charge rate in watts (nullable).
    /// </summary>
    public int? ChargeRateWatts { get; set; }

    /// <summary>
    /// Gets or sets the lock status.
    /// </summary>
    public LockStatus LockStatus { get; set; } = LockStatus.Unknown;

    /// <summary>
    /// Gets or sets the charger status.
    /// </summary>
    public ChargerStatus ChargerStatus { get; set; } = ChargerStatus.Unknown;

    /// <summary>
    /// Gets or sets the charging status.
    /// </summary>
    public ChargingStatus ChargingStatus { get; set; } = ChargingStatus.Unknown;

    /// <summary>
    /// Gets or sets the Zappi mode.
    /// </summary>
    public ZappiMode ZappiMode { get; set; } = ZappiMode.Unknown;

    /// <summary>
    /// Gets or sets a value indicating whether daylight saving is active.
    /// </summary>
    public bool IsDayLightsSaving { get; internal set; }

    /// <summary>
    /// Returns a string representation of the current object.
    /// </summary>
    public override string ToString()
    {
        return $"Added:{ChargeAddedKWh}KWh, Rate:{ChargeRateWatts}W, Charger:{ChargerStatus}, Charging:{ChargingStatus}, Mode:{ZappiMode}, Lock:{LockStatus}";
    }
}
