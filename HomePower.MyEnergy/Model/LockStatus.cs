namespace HomePower.MyEnergy.Model;

/// <summary>
/// Lock Status
/// Bit 0: Locked Now
/// Bit 1: Lock when plugged in
/// Bit 2: Lock when unplugged.
/// Bit 3: Charge when locked.
/// Bit 4: Charge Session Allowed(Even if locked)
/// </summary>
public enum LockStatus
{
    LockedNow = 1,
    LockWhenPluggedIn = 2,
    LockWhenUnplugged = 4,
    ChargeWhenLocked = 8,
    ChargeSessionAllowed = 16,
    Unknown = 32
}
