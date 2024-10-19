using HomePower.MyEnergy.Dto;

namespace HomePower.MyEnergy.Model;
internal static class MappingExtensions
{
    public static EvChargeStatus ToEvChargeStatus(this ZappiDto zappi)
    {
        return new EvChargeStatus
        {
            ChargeAddedKWh = zappi.ChargeAddedKWh,
            ChargeRateWatts = zappi.ChargeRateWatts,
            ChargerStatus = zappi.ChargerStatus.ToChargerStatus(),
            ChargingStatus = zappi.ChargingStatus.ToChargingStatus(),
            LockStatus = zappi.LockStatus.ToLockStatus(),
            ZappiMode = zappi.ZappiMode.ToZappiMode()
        };
    }

    private static ChargerStatus ToChargerStatus(this string chargeStatus)
        => chargeStatus switch
        {
            "A" => ChargerStatus.EvDisconnected,
            "B1" => ChargerStatus.EvConnected,
            "B2" => ChargerStatus.WaitingForEv,
            "C1" => ChargerStatus.EvReady,
            "C2" => ChargerStatus.Charging,
            "F" => ChargerStatus.Fault,
            _ => ChargerStatus.Unknown
        };

    private static ChargingStatus ToChargingStatus(this int chargingStatus)
        => chargingStatus switch
        {
            1 => ChargingStatus.Paused,
            3 => ChargingStatus.Diverting_Charging,
            4 => ChargingStatus.Waiting,
            5 => ChargingStatus.Complete,
            _ => ChargingStatus.Unknown
        };

    private static LockStatus ToLockStatus(this int lockStatus)
        => lockStatus switch
        {
            1 => LockStatus.LockedNow,
            2 => LockStatus.LockWhenPluggedIn,
            4 => LockStatus.LockWhenUnplugged,
            8 => LockStatus.ChargeWhenLocked,
            16 => LockStatus.ChargeSessionAllowed,
            _ => LockStatus.Unknown
        };

    private static ZappiMode ToZappiMode(this int zappiMode)
        => zappiMode switch
        {
            0 => ZappiMode.Startup_Fault,
            1 => ZappiMode.Fast,
            2 => ZappiMode.Eco,
            3 => ZappiMode.EcoPlus,
            4 => ZappiMode.Stopped,
            _ => ZappiMode.Unknown
        };
}
