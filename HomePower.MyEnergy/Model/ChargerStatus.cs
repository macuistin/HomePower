namespace HomePower.MyEnergy.Model;

/// <summary>
/// Charger Status
/// </summary>
public enum ChargerStatus
{
    EvDisconnected,
    EvConnected,
    WaitingForEv,
    EvReady,
    Charging,
    Fault,
    Unknown
}
