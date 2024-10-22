namespace HomePower.GivEnergy.Client;

/// <summary>
/// Enumeration for setting IDs and their corresponding validation types.
/// </summary>
public enum InverterSettingId
{
    /// <summary>
    /// Enable AC Charge Upper % Limit - Boolean (true/false).
    /// </summary>
    EnableACChargeUpperPercentLimit = 17, // boolean

    /// <summary>
    /// Enable Eco Mode - Boolean (true/false).
    /// </summary>
    EnableEcoMode = 24, // boolean

    /// <summary>
    /// DC Discharge 2 Start Time - Value format should be HH:mm.
    /// </summary>
    DCDischarge2StartTime = 41, // string, time, HH:mm

    /// <summary>
    /// DC Discharge 2 End Time - Value format should be HH:mm.
    /// </summary>
    DCDischarge2EndTime = 42, // string, time, HH:mm

    /// <summary>
    /// Inverter Max Output Active Power Percent - Value must be between 0 and 100.
    /// </summary>
    InverterMaxOutputActivePowerPercent = 47, // between 0-100

    /// <summary>
    /// DC Discharge 1 Start Time - Value format should be HH:mm.
    /// </summary>
    DCDischarge1StartTime = 53, // string, time, HH:mm

    /// <summary>
    /// DC Discharge 1 End Time - Value format should be HH:mm.
    /// </summary>
    DCDischarge1EndTime = 54, // string, time, HH:mm

    /// <summary>
    /// Enable DC Discharge - Boolean (true/false).
    /// </summary>
    EnableDCDischarge = 56, // boolean

    /// <summary>
    /// AC Charge 1 Start Time - Value format should be HH:mm.
    /// </summary>
    ACCharge1StartTime = 64,   // string, time, HH:mm

    /// <summary>
    /// AC Charge 1 End Time - Value format should be HH:mm.
    /// </summary>
    ACCharge1EndTime = 65,     // string, time, HH:mm

    /// <summary>
    /// AC Charge Enable - Boolean (true/false).
    /// </summary>
    ACChargeEnable = 66,       // boolean

    /// <summary>
    /// Battery Reserve % Limit - Value must be between 4 and 100.
    /// </summary>
    BatteryReservePercentLimit = 71, // between 4-100

    /// <summary>
    /// Battery Charge Power - Value must be between 0 and 3600.
    /// </summary>
    BatteryChargePower = 72, // between 0-3600

    /// <summary>
    /// Battery Discharge Power - Value must be between 0 and 3600.
    /// </summary>
    BatteryDischargePower = 73, // between 0-3600

    /// <summary>
    /// Battery Cutoff % Limit - Value must be between 4 and 100.
    /// </summary>
    BatteryCutoffPercentLimit = 75, // between 4-100

    /// <summary>
    /// AC Charge Upper % Limit - Value must be between 0 and 100.
    /// </summary>
    ACChargeUpperPercentLimit = 77, // between 0-100

    /// <summary>
    /// Restart Inverter - Value can only be 100.
    /// </summary>
    RestartInverter = 83, // exact:100

    /// <summary>
    /// Pause Battery - Value must be one of: 0 (Not Paused), 1 (Pause Charge), 2 (Pause Discharge), 3 (Pause Charge & Discharge).
    /// </summary>
    PauseBattery = 96, // in:0,1,2,3

    /// <summary>
    /// AC Charge 1 Upper SOC % Limit - Value must be between 0 and 100.
    /// </summary>
    ACCharge1UpperSOCPercentLimit = 101, // between 0-100

    /// <summary>
    /// AC Charge 2 Start Time - Value format should be HH:mm.
    /// </summary>
    ACCharge2StartTime = 102, // string, time, HH:mm

    /// <summary>
    /// AC Charge 2 End Time - Value format should be HH:mm.
    /// </summary>
    ACCharge2EndTime = 103, // string, time, HH:mm

    /// <summary>
    /// AC Charge 2 Upper SOC % Limit - Value must be between 0 and 100.
    /// </summary>
    ACCharge2UpperSOCPercentLimit = 104, // between 0-100

    /// <summary>
    /// AC Charge 3 Start Time - Value format should be HH:mm.
    /// </summary>
    ACCharge3StartTime = 105, // string, time, HH:mm

    /// <summary>
    /// AC Charge 3 End Time - Value format should be HH:mm.
    /// </summary>
    ACCharge3EndTime = 106, // string, time, HH:mm

    /// <summary>
    /// AC Charge 3 Upper SOC % Limit - Value must be between 0 and 100.
    /// </summary>
    ACCharge3UpperSOCPercentLimit = 107, // between 0-100

    /// <summary>
    /// AC Charge 4 Start Time - Value format should be HH:mm.
    /// </summary>
    ACCharge4StartTime = 108, // string, time, HH:mm

    /// <summary>
    /// AC Charge 4 End Time - Value format should be HH:mm.
    /// </summary>
    ACCharge4EndTime = 109, // string, time, HH:mm

    /// <summary>
    /// AC Charge 4 Upper SOC % Limit - Value must be between 0 and 100.
    /// </summary>
    ACCharge4UpperSOCPercentLimit = 110, // between 0-100

    /// <summary>
    /// DC Discharge 1 Lower SOC % Limit - Value must be between 0 and 100.
    /// </summary>
    DCDischarge1LowerSOCPercentLimit = 129, // between 0-100

    /// <summary>
    /// Enable EPS - Boolean (true/false).
    /// </summary>
    EnableEPS = 271, // boolean

    /// <summary>
    /// Inverter Charge Power Percentage - Value must be between 1 and 100.
    /// </summary>
    InverterChargePowerPercentage = 267, // between 1-100

    /// <summary>
    /// Inverter Discharge Power Percentage - Value must be between 1 and 100.
    /// </summary>
    InverterDischargePowerPercentage = 268 // between 1-100
}
