namespace HomePower.MyEnergi.Model;

/// <summary>
/// Represents the status of Zappi devices.
/// </summary>
public record EvChargeStatus
{
    public static readonly EvChargeStatus Failed = new();

    /// <summary>
    /// Charge added in KWh.
    /// </summary>
    public double ChargeAddedKWh { get; set; }

    /// <summary>
    /// Charge Rate in Watts (nullable).
    /// </summary>
    public int? ChargeRateWatts { get; set; }

    public LockStatus LockStatus { get; set; } = LockStatus.Unknown;

    public ChargerStatus ChargerStatus { get; set; } = ChargerStatus.Unknown;

    public ChargingStatus ChargingStatus { get; set; } = ChargingStatus.Unknown;

    public ZappiMode ZappiMode { get; set; } = ZappiMode.Unknown;

    public override string ToString()
    {
        return $"ChargeAddedKWh: {ChargeAddedKWh}, ChargeRateWatts: {ChargeRateWatts}, ChargerStatus: {ChargerStatus}, ChargingStatus: {ChargingStatus}, LockStatus: {LockStatus}, ZappiMode: {ZappiMode}";
    }

}