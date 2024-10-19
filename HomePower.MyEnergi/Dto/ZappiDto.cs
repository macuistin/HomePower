using System.Text.Json.Serialization;

namespace HomePower.MyEnergi.Dto;

/// <summary>
/// Represents a Zappi device.
/// </summary>
public record ZappiDto
{
    /// <summary>
    /// Device Class.
    /// </summary>
    [JsonPropertyName("deviceClass")]
    public string DeviceClass { get; set; }

    /// <summary>
    /// Charge added in KWh.
    /// </summary>
    [JsonPropertyName("che")]
    public double ChargeAddedKWh { get; set; }

    /// <summary>
    /// Command Timer
    /// 
    /// Counts 1 - 10 when command LockStatussent, 
    /// then 254 - success, 
    /// 253 - failure, 
    /// 255 - never received any commands.
    /// </summary>
    [JsonPropertyName("cmt")]
    public int CommandTimer { get; set; }

    /// <summary>
    /// Date in dd-mm-YYYY format.
    /// </summary>
    [JsonPropertyName("dat")]
    public string Date { get; set; }

    /// <summary>
    /// Charge Rate in Watts (nullable).
    /// </summary>
    [JsonPropertyName("div")]
    public int? ChargeRateWatts { get; set; }

    /// <summary>
    /// Indicates if Daylight Savings Time is enabled.
    /// </summary>
    [JsonPropertyName("dst")]
    public int UseDaylightSavingsTime { get; set; }

    /// <summary>
    /// Physical CT connection 1 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp1")]
    public int PhysicalCTConnectionWatts1 { get; set; }

    /// <summary>
    /// Physical CT connection 2 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp2")]
    public int PhysicalCTConnectionWatts2 { get; set; }

    /// <summary>
    /// Physical CT connection 3 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp3")]
    public int PhysicalCTConnectionWatts3 { get; set; }

    /// <summary>
    /// Physical CT connection 4 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp4")]
    public int PhysicalCTConnectionWatts4 { get; set; }

    /// <summary>
    /// Physical CT connection 5 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp5")]
    public int PhysicalCTConnectionWatts5 { get; set; }

    /// <summary>
    /// Physical CT connection 6 value in Watts.
    /// </summary>
    [JsonPropertyName("ectp6")]
    public int PhysicalCTConnectionWatts6 { get; set; }

    /// <summary>
    /// CT Clamp 1 Name.
    /// </summary>
    [JsonPropertyName("ectt1")]
    public string CT1Name { get; set; }

    /// <summary>
    /// CT Clamp 2 Name.
    /// </summary>
    [JsonPropertyName("ectt2")]
    public string CT2Name { get; set; }

    /// <summary>
    /// CT Clamp 3 Name.
    /// </summary>
    [JsonPropertyName("ectt3")]
    public string CT3Name { get; set; }

    /// <summary>
    /// CT Clamp 4 Name.
    /// </summary>
    [JsonPropertyName("ectt4")]
    public string CT4Name { get; set; }

    /// <summary>
    /// CT Clamp 5 Name.
    /// </summary>
    [JsonPropertyName("ectt5")]
    public string CT5Name { get; set; }

    /// <summary>
    /// CT Clamp 6 Name.
    /// </summary>
    [JsonPropertyName("ectt6")]
    public string CT6Name { get; set; }

    /// <summary>
    /// Supply Frequency.
    /// </summary>
    [JsonPropertyName("frq")]
    public double SupplyFrequency { get; set; }

    /// <summary>
    /// Firmware Version.
    /// </summary>
    [JsonPropertyName("fwv")]
    public string FirmwareVersion { get; set; }

    /// <summary>
    /// Generated Watts.
    /// </summary>
    [JsonPropertyName("gen")]
    public int GeneratedWatts { get; set; }

    /// <summary>
    /// Watts from grid.
    /// </summary>
    [JsonPropertyName("grd")]
    public int WattsFromGrid { get; set; }

    /// <summary>
    /// Lock Status
    /// Bit 0: Locked Now
    /// Bit 1: Lock when plugged in
    /// Bit 2: Lock when unplugged.
    /// Bit 3: Charge when locked.
    /// Bit 4: Charge Session Allowed(Even if locked)
    /// </summary>
    [JsonPropertyName("lck")]
    public int LockStatus { get; set; }

    /// <summary>
    /// Minimum Green Level (%).
    /// </summary>
    [JsonPropertyName("mgl")]
    public int MinimumGreenLevel { get; set; }

    /// <summary>
    /// Phases.
    /// </summary>
    [JsonPropertyName("pha")]
    public int Phases { get; set; }

    /// <summary>
    /// Priority.
    /// </summary>
    [JsonPropertyName("pri")]
    public int Priority { get; set; }

    /// <summary>
    /// Charge Status
    /// 
    /// A: EV Unplugged
    /// B1: EV Connected
    /// B2: Waiting EV
    /// C1: EV ready
    /// C2: Charging
    /// F: RED_Fault
    /// U: Unknown
    /// </summary>
    [JsonPropertyName("pst")]
    public string ChargerStatus { get; set; }

    /// <summary>
    /// Smart Boost Start Time Hour.
    /// </summary>
    [JsonPropertyName("sbh")]
    public int SmartBoostStartTimeHour { get; set; }

    /// <summary>
    /// Smart Boost KWh to add.
    /// </summary>
    [JsonPropertyName("sbk")]
    public int SmartBoostKWhToAdd { get; set; }

    /// <summary>
    /// Smart Boost Start Time Minute.
    /// </summary>
    [JsonPropertyName("sbm")]
    public int SmartBoostStartTimeMinute { get; set; }

    /// <summary>
    /// Zappi Serial Number.
    /// </summary>
    [JsonPropertyName("sno")]
    public long ZappiSerialNumber { get; set; }

    /// <summary>
    /// Status
    /// 1: Paused
    /// 3: Diverting/Charging
    /// 4: Waiting
    /// 5: Complete
    /// </summary>
    [JsonPropertyName("sta")]
    public int ChargingStatus { get; set; }

    /// <summary>
    /// Boost hour.
    /// </summary>
    [JsonPropertyName("tbh")]
    public int BoostHour { get; set; }

    /// <summary>
    /// Boost KWh.
    /// </summary>
    [JsonPropertyName("tbk")]
    public int BoostKWh { get; set; }

    /// <summary>
    /// Boost minute.
    /// </summary>
    [JsonPropertyName("tbm")]
    public int BoostMinute { get; set; }

    /// <summary>
    /// Time in hh:mm:ss format.
    /// </summary>
    [JsonPropertyName("tim")]
    public string Time { get; set; }

    /// <summary>
    /// Supply voltage.
    /// </summary>
    [JsonPropertyName("vol")]
    public double SupplyVoltage { get; set; }

    /// <summary>
    /// Zappi Mode
    /// 
    /// 1: Fast
    /// 2: Eco
    /// 3: Eco+
    /// 4: Stopped
    /// </summary>
    [JsonPropertyName("zmo")]
    public int ZappiMode { get; set; }

    /// <summary>
    /// Boost start minute.
    /// </summary>
    [JsonPropertyName("bsm")]
    public int BoostStartMinute { get; set; }

    /// <summary>
    /// Boost Time Remaining.
    /// </summary>
    [JsonPropertyName("bst")]
    public int BoostTimeRemaining { get; set; }

    /// <summary>
    /// Time Zone.
    /// </summary>
    [JsonPropertyName("tz")]
    public int TimeZone { get; set; }

    /// <summary>
    /// Indicates if VHub is enabled.
    /// </summary>
    [JsonPropertyName("isVHubEnabled")]
    public bool IsVHubEnabled { get; set; }

    /// <summary>
    /// To be confirmed.
    /// It came back from the changer, but I don't know what it is
    /// </summary>
    [JsonPropertyName("bss")]
    public int Bss { get; set; }

    /// <summary>
    /// Pulse Width Modulation (duty cycle). Controls the charge rate to an electric vehicle.
    /// </summary>
    [JsonPropertyName("pwm")]
    public int PulseWidthModulation { get; set; }

    /// <summary>
    /// Zappi Status.
    /// </summary>
    [JsonPropertyName("zs")]
    public int ZappiStatus { get; set; }

    /// <summary>
    /// Reduced Current. Indicates if the current being supplied is reduced.
    /// 
    /// Could be residual DC current.
    /// </summary>
    [JsonPropertyName("rdc")]
    public int ReducedCurrent { get; set; }

    /// <summary>
    /// Relay Active Count. The number of times the relay has been activated.
    /// </summary>
    [JsonPropertyName("rrac")]
    public int RelayActiveCount { get; set; }

    /// <summary>
    /// Zappi Shunt. Indicates current flow through the Zappi charger.
    /// </summary>
    [JsonPropertyName("zsh")]
    public int ZappiShunt { get; set; }

    /// <summary>
    /// Zappi being tampered with.
    /// </summary>
    [JsonPropertyName("beingTamperedWith")]
    public bool BeingTamperedWith { get; set; }

    /// <summary>
    /// Battery Discharge Enabled.
    /// </summary>
    [JsonPropertyName("batteryDischargeEnabled")]
    public bool BatteryDischargeEnabled { get; set; }

    /// <summary>
    /// G100 Lockout State.
    /// </summary>
    [JsonPropertyName("g100LockoutState")]
    public string G100LockoutState { get; set; }

    /// <summary>
    /// Phase Setting.
    /// </summary>
    [JsonPropertyName("phaseSetting")]
    public string PhaseSetting { get; set; }

    /// <summary>
    /// Wifi Link enabled.
    /// </summary>
    [JsonPropertyName("wifiLink")]
    public bool WifiLink { get; set; }

    /// <summary>
    /// Ethernet Link enabled.
    /// </summary>
    [JsonPropertyName("ethernetLink")]
    public bool EthernetLink { get; set; }

    /// <summary>
    /// Open Charge Point Protocol (OCPP) enabled.
    /// </summary>
    [JsonPropertyName("ocppEnabled")]
    public bool OcppEnabled { get; set; }

    /// <summary>
    /// New App Available.
    /// </summary>
    [JsonPropertyName("newAppAvailable")]
    public bool NewAppAvailable { get; set; }

    /// <summary>
    /// New Bootloader Available.
    /// </summary>
    [JsonPropertyName("newBootloaderAvailable")]
    public bool NewBootloaderAvailable { get; set; }

    /// <summary>
    /// Product Code.
    /// </summary>
    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; }
}
