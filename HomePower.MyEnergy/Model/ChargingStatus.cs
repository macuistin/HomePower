namespace HomePower.MyEnergy.Model;

/// <summary>
/// Status
/// </summary>
public enum ChargingStatus
{
    Paused = 1,
    Diverting_Charging = 3,
    Waiting = 4,
    Complete = 5,
    Unknown = 6,
}
