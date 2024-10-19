namespace HomePower.GivEnergi.Service;

public interface IGivEnergyService
{
    /// <summary>
    /// Is the AC battery charging enabled
    /// </summary>
    /// <returns>boolean</returns>
    Task<bool> GetACChargeEnabledAsync();
    
    /// <summary>
    /// Time the AC battery charging ends
    /// </summary>
    /// <returns>Time AC charging ends,HH:mm</returns>
    Task<string> GetBatteryChargeEndTimeAsync();

    /// <summary>
    /// Time the AC battery charging starts
    /// </summary>
    /// <returns>Time AC charging starts,HH:mm</returns>

    Task<string> GetBatteryChargeStartTimeAsync();
    
    
    Task<bool> UpdateACChargeEnabledAsync(bool enabled);
 
    Task<bool> UpdateBatteryChargeEndTimeAsync(int hour, int minute);
    
    Task<bool> UpdateBatteryChargeStartTimeAsync(int hour, int minute);
}